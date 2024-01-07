using JWT_Authentication_With_RefreshToken.DAL;
using JWT_Authentication_With_RefreshToken.EL;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace JWT_Authentication_With_RefreshToken.BL
{
    public class JWTAuthenticationWithRefreshTokenBL : IJWTAuthenticationWithRefreshTokenBL
    {
        private readonly IJWTAuthenticationWithRefreshTokenDAL jWTAuthenticationWithRefreshTokenDAL;
        public JWTAuthenticationWithRefreshTokenBL(IJWTAuthenticationWithRefreshTokenDAL _jWTAuthenticationWithRefreshTokenDAL)
        {
            jWTAuthenticationWithRefreshTokenDAL = _jWTAuthenticationWithRefreshTokenDAL;
        }
        public async Task<AuthenticationResponse> AuthenticateUserBL(string userId, string password, IConfiguration configuration)
        {
            #region LocalVariables
            AuthenticationResponse response = null;
            string responseJson = string.Empty;
            DBConfig dBConfig = new DBConfig();
            var responseTemplate = new { userId = string.Empty, IsAuthenticated = false, authenticationMessage = string.Empty };
            #endregion
            try
            {
                dBConfig.ConnectionString = configuration.GetConnectionString("DBConnectionString");
                dBConfig.storedProc = configuration.GetSection("SPconfig")["SP_AuthenticateUser"];

                responseJson = await jWTAuthenticationWithRefreshTokenDAL.AuthenticateUserDAL(userId, password, dBConfig);

                if (!string.IsNullOrEmpty(responseJson))
                {
                    var businessData = JsonConvert.DeserializeAnonymousType(responseJson, responseTemplate);

                    response = new AuthenticationResponse()
                    {
                        UserId = businessData.userId,
                        Password = string.Empty,
                        IsValid = businessData.IsAuthenticated,
                        RefreshToken = string.Empty,
                        AuthenticationMessage = businessData.authenticationMessage

                    };
                }
                else
                {
                    response = null; //It means exception might have occured in down loayers or Stored proc, hence data has not come. We will using null validation in controller to generate 500 code.
                }

            }
            catch(Exception ex)
            {
                //log this exception and pass the flow to controller catch block to generate 500 error response.
                response = null;
                responseJson = string.Empty;
                throw;
            }
            return response;
        }
        public async Task<string> GenerateRefreshToken(string ipAddr, string userId, IConfiguration config)
        {
            #region Local Variables
            string refreshToken = string.Empty;
            string refreshTokenJson = string.Empty;
            DBConfig dBConfig = new DBConfig();
            string responseDAL = string.Empty;
            var spResponseTemplate = new { RFTokenSaved = true, Message = String.Empty };
            bool flag = false;
            #endregion

            try
            {
                refreshToken = GetRefreshToken();

                var refreshTokenObj = new
                {
                    UserId = userId,
                    RefreshToken = refreshToken,
                    ExpirationDate = DateTime.Now.AddDays(7).Date.ToString("yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture), //to get date in this example format : 2023-11-18 00:00:00
                    CreatedByIp = ipAddr
                };

                refreshTokenJson = JsonConvert.SerializeObject(refreshTokenObj);

                dBConfig.storedProc = config.GetSection("SPconfig")["SP_SaveRefreshToken"];
                dBConfig.ConnectionString = config.GetConnectionString("DBConnectionString");

                responseDAL = await jWTAuthenticationWithRefreshTokenDAL.SaveRefreshTokenDAL(refreshTokenJson, dBConfig);
                if (responseDAL != string.Empty)
                {
                    //deserilize json data from proc.
                    var spResponseObj = JsonConvert.DeserializeAnonymousType(responseDAL, spResponseTemplate);
                    flag = spResponseObj.RFTokenSaved;

                    if (!flag) //It means refresh token did not get saved in DB table as per proc logic
                    {
                        refreshToken = string.Empty;
                    }
                }
                else
                {
                    refreshToken = string.Empty;
                }

            }
            catch(Exception ex)
            {
                //Log this exception
                refreshToken = string.Empty;
                throw;
            }

            return refreshToken;
        }
        private string GetRefreshToken()
        {
            /// <summary>
            /// generate token that is validity of 7 days
            ///The method takes a string parameter called ipAddress, which is the IP address of the client requesting the refresh token.
            ///The method creates an instance of the RNGCryptoServiceProvider class, which is a cryptographic random number generator.
            ///The method declares an array of 64 bytes called randomBytes and fills it with random data using the GetBytes method of the RNGCryptoServiceProvider instance.
            ///The method converts the randomBytes array to a base64 string using the Convert.ToBase64String method.This string is assigned to the Token property of a new RefreshToken object.
            /// </summary>

            /// <returns>
            /// string
            /// </returns>

            var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[64];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
