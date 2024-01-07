using JWT_Authentication_With_RefreshToken.EL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWT_Authentication_With_RefreshToken.Utility
{
    public class ResponseHandler
    {
        private static string errMessage = "An error occured while processing your request. Please contact system admin";
        public static AppResponse CreateErrorResponse()
        {
            AppResponse response = new AppResponse();
            response.Header = new ResponseHeader() { Status = "Failed" };
            response.Body = new ResponseBody() { BusinessData = string.Empty, OtherInformation = string.Empty };
            response.Error = new Error() { ErrorNo = string.Empty, ErrorMessage = errMessage };

            return response;
        }

        public static AppResponse CreateErrorResponse(string message)
        {
            AppResponse response = new AppResponse();
            response.Header = new ResponseHeader() { Status = "Failed" };
            response.Body = new ResponseBody() { BusinessData = string.Empty, OtherInformation = string.Empty };
            response.Error = new Error() { ErrorNo = string.Empty, ErrorMessage = message };

            return response;
        }
    }
}
