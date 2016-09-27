using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelstraApp.Core.ViewModels
{
    public class AddRequest
    {
        public string UserNameReq { get; set; }
        public string UserStatus { get; set; }

        public string ReqStatus(bool status)
        {
            if (status)
            {
                return "Has responded";
            }
            return "Has not responded";
        }

        public AddRequest() { }
        public AddRequest(string userReq, bool userStatus)
        {
            UserNameReq = userReq;
            UserStatus = ReqStatus(userStatus);
        }
        public AddRequest(string userReq)
        {
            UserNameReq = userReq;
            UserStatus = "Has not responded";
        }
    }


}
