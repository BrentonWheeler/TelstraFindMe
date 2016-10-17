using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelstraApp.Core.ViewModels
{
    public class ReceivedRequest
    {
        public string RequestersName { get; set; }
        public DateTime RequestersStatus { get; set; }

        public string ReqStatus(bool status)
        {
            if (status)
            {
                return "Has responded";
            }
            return "Has not responded";
        }

        public ReceivedRequest() { }
        public ReceivedRequest(string userReq, DateTime userStatus)
        {
            RequestersName = userReq;
            RequestersStatus = userStatus;
        }
    }


}
