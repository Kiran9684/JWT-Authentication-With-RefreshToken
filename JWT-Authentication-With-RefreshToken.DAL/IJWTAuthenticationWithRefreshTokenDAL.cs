using JWT_Authentication_With_RefreshToken.EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWT_Authentication_With_RefreshToken.DAL
{
    public interface IJWTAuthenticationWithRefreshTokenDAL
    {
        public Task<string> AuthenticateUserDAL(string userId, string password, DBConfig dBConfig);
        public Task<string> SaveRefreshTokenDAL(string refreshTokenJson, DBConfig dbConfig);
    }
}
