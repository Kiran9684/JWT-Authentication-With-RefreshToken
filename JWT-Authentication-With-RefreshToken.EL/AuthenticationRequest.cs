﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWT_Authentication_With_RefreshToken.EL
{
    public class AuthenticationRequest
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string Operation { get; set; }
    }
}
