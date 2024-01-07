using JWT_Authentication_With_RefreshToken.EL;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace JWT_Authentication_With_RefreshToken.BL
{
    public interface IJWTAuthenticationWithRefreshTokenBL
    {
        public Task<AuthenticationResponse> AuthenticateUserBL(string userId, string password, IConfiguration configuration);
        public Task<string> GenerateRefreshToken(string ipAddr, string userId, IConfiguration config);
    }
}
