using JWT_Authentication_With_RefreshToken.BL;
using JWT_Authentication_With_RefreshToken.DAL;

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
            builder.Services.AddSingleton<IJWTAuthenticationWithRefreshTokenBL,JWTAuthenticationWithRefreshTokenBL>();
            builder.Services.AddSingleton<IJWTAuthenticationWithRefreshTokenDAL,JWTAuthenticationWithRefreshTokenDAL>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}