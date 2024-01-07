using JWT_Authentication_With_RefreshToken.BL;
using JWT_Authentication_With_RefreshToken.DAL;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace JWT_Authentication_With_RefreshToken.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<IJWTAuthenticationWithRefreshTokenBL, JWTAuthenticationWithRefreshTokenBL>();
            builder.Services.AddSingleton<IJWTAuthenticationWithRefreshTokenDAL, JWTAuthenticationWithRefreshTokenDAL>();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer((options) =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    //You can set this to true for production.
                    //This setting determines whether to validate the "issuer" (iss) claim of the JWT. The "issuer" is the entity that issued the JWT. In this code, it's set to true, which means the issuer will be validated. In a production environment, you should typically set this to true to ensure that the token is issued by a trusted authority.
                    ValidateAudience = true,
                    //The "audience" (aud) claim in a JWT (JSON Web Token) represents the intended recipient of the token. 
                    //this setting determines whether to validate the "audience" (aud) claim of the JWT. The "audience" represents the intended recipient of the JWT. Again, it's set to true, but it's advisable to set it to true in production to verify that the token is meant for your application.

                    ValidateLifetime = true,
                    // This setting ensures that the token has not expired. It's set to true so that the token's expiration time is checked during validation.

                    ValidateIssuerSigningKey = true,
                    //This setting determines whether to validate the signing key used to sign the JWT. Setting it to true ensures that the token's signature is verified with the specified key.

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT")["SecretKey"]))
                    //This line specifies the key used to verify the token's signature. It's set to a SymmetricSecurityKey created from a secret key. In a real application, you should replace "YourSecretKey" with a strong and secret key. The same key should be used for signing tokens in your authentication process.
                };

                //As we specified to validate Issuer and Audience, we must also specify the details of Audience and Issuer to validate the incoming token's issuer and audience against these details. 
                options.TokenValidationParameters.ValidAudience = builder.Configuration.GetSection("JWT")["Audience"];
                options.TokenValidationParameters.ValidIssuer = builder.Configuration.GetSection("JWT")["Issuer"];
            });
            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}