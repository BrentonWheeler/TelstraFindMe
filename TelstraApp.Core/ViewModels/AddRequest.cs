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


        public AddRequest() { }
        public AddRequest(string unitCode, string unitName)
        {
            UserNameReq = unitCode;
            UserStatus = unitName;
        }
        public AddRequest(string unitCode)
        {
            UserNameReq = unitCode;
            UserStatus = "Has not responded";
        }
    }


}
