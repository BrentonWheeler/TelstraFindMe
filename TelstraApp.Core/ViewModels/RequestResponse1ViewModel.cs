using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelstraApp.Core.Models;

namespace TelstraApp.Core.ViewModels
{

    public class RequestResponse1ViewModel : MvxViewModel
    {
        private string responseMsg;
        public string ResponseMsg
        {
            get
            {
                return responseMsg;
            }

            set
            {
                SetProperty(ref responseMsg, value);
            }
        }
        public void Init(Users response)
        {
            BuildLocationMessage message = new BuildLocationMessage(response);

            ResponseMsg = message.message;
        }

        public RequestResponse1ViewModel(){}

    }
}
