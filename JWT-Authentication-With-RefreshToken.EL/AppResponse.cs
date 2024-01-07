using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWT_Authentication_With_RefreshToken.EL
{
    public class AppResponse
    {
        public ResponseHeader Header { get; set; }
        public ResponseBody Body { get; set; }
        public Error Error { get; set; }
    }

    public class ResponseHeader
    {
        public string Status { get; set; }
    }

    public class ResponseBody
    {
        public string BusinessData { get; set; }
        public string OtherInformation { get; set; }
    }

    public class Error
    {
        public string ErrorNo { get; set; }
        public string ErrorMessage { get; set; }
    }
}
