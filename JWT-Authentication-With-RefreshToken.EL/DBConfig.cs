using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace JWT_Authentication_With_RefreshToken.EL
{
    public class DBConfig
    {
        public string ConnectionString { get; set; }
        public string storedProc { get; set; }
    }
}
