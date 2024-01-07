using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWT_Authentication_With_RefreshToken.EL
{
    public class AuthenticationResponse
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string JWTToken { get; set; }
        public string RefreshToken { get; set; }
        public bool IsValid { get; set; }
        public string AuthenticationMessage { get; set; }
    }
}
