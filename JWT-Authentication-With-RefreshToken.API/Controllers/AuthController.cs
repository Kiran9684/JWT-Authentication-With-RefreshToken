using JWT_Authentication_With_RefreshToken.BL;
using JWT_Authentication_With_RefreshToken.EL;
using JWT_Authentication_With_RefreshToken.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWT_Authentication_With_RefreshToken.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IConfiguration configuration;
        private IJWTAuthenticationWithRefreshTokenBL jWTAuthenticationWithRefreshTokenBL;

        public AuthController(IConfiguration _configuration, IJWTAuthenticationWithRefreshTokenBL _jWTAuthenticationWithRefreshTokenBL) //Constructor Dependency Injection
        {
            configuration = _configuration;
            jWTAuthenticationWithRefreshTokenBL = _jWTAuthenticationWithRefreshTokenBL;
        }

        [HttpPost]
        [Route("user-login")]
        public async Task<ActionResult<AuthenticationResponse>> UserAuthentication([FromBody]AuthenticationRequest authenticationRequest)
        {
            #region LocalVariables
            ActionResult authResult = null;
            AuthenticationResponse authenticationResponse = null;
            string ipAddress = string.Empty;
            string refreshToken = string.Empty;
            #endregion
            try
            {
                ipAddress = Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString(); //Read ip address of the system in which app is being used.

                if(string.IsNullOrEmpty(authenticationRequest.UserId) || string.IsNullOrEmpty(authenticationRequest.Password))
                {
                    authResult = BadRequest(ResponseHandler.CreateErrorResponse("User Id OR Password Cannot Be Empty"));
                }
                else
                {
                    authenticationResponse = await jWTAuthenticationWithRefreshTokenBL.AuthenticateUserBL(authenticationRequest.UserId, authenticationRequest.Password, configuration);

                    if (authenticationResponse != null)
                    {
                        //Write code for Generating JWT generation and refresh token generation
                        if (authenticationResponse.IsValid)
                        {
                            authenticationResponse.JWTToken = generateJWTToken(authenticationResponse.UserId);

                            refreshToken = await jWTAuthenticationWithRefreshTokenBL.GenerateRefreshToken(ipAddress, authenticationResponse.UserId, configuration);

                            setCookie(refreshToken);

                            authResult = Ok(authenticationResponse);
                        }
                        else if(!authenticationResponse.IsValid)
                        {
                            //If user is in-valid , return 401 Unauthorize response with Invalid User Id and Password message.
                            clearCookie("refreshToken");
                            authResult = StatusCode(401, ResponseHandler.CreateErrorResponse(authenticationResponse.AuthenticationMessage));
                        }
                    }
                    else
                    {
                        authResult = StatusCode(500, ResponseHandler.CreateErrorResponse());
                    }
                }
            }
            catch(Exception ex)
            {
                //You can log this exception for logs.
                //We are not sending exception detail to front-end as it may contain some sensitive info which is not supposed to be exposed. 
                //Instead, we send custom 500 error response.
                authResult = StatusCode(500, ResponseHandler.CreateErrorResponse());
            }

            return authResult;
        }

        [NonAction]
        private string generateJWTToken(string userId)
        {
            #region Local Variables
            string JWTToken = string.Empty;
            #endregion

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(configuration.GetSection("JWT")["SecretKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Audience = configuration.GetSection("JWT")["Audience"],
                Issuer = configuration.GetSection("JWT")["Issuer"],
                Subject = new ClaimsIdentity(new[] {
                    new Claim("sub",userId)
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt16(configuration.GetSection("JWT")["JwtTokenExpiry"]))
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            JWTToken = tokenHandler.WriteToken(token);

            return JWTToken;

            /*
                One more approach, Iam commenting it for documentation and future reference: 

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JWT")["SecretKey"]));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub,user.UserId),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
                };

                var token = new JwtSecurityToken(claims: claims, signingCredentials: credentials, expires: DateTime.UtcNow.AddMinutes(15));
                return new JwtSecurityTokenHandler().WriteToken(token);
             */
        }

        [NonAction]
        private void setCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions()
            {
                HttpOnly = true,//It means that the cookie that is set by the code is protected from being read or modified by any JavaScript code that runs on the web page. This is a security feature that helps prevent cross-site scripting (XSS) attacks, which are a type of web attack where malicious code is injected into a web page and executed by the browser. By setting the HttpOnly flag on the cookie, the server tells the browser that the cookie should only be sent back to the server in HTTP requests, and not exposed to any other APIs, such as document.cookie
                Expires = DateTime.Now.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }

        [NonAction]
        private void clearCookie(string key)
        {
            Response.Cookies.Delete(key);
        }
    }
}
