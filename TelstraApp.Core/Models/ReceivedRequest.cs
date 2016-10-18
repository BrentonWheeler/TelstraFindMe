using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelstraApp.Core.ViewModels
{
    public class ReceivedRequest : MvxViewModel
    {
        private Boolean _liisChecked = true;
        public bool LIIsChecked
        {
            get { return this._liisChecked; }
            set
            {
                _liisChecked = !_liisChecked;
                RaisePropertyChanged(() => LIIsChecked);
            }
        }

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

        public void checkBox(bool isChecked)
        {
            _liisChecked = isChecked;
            RaisePropertyChanged(() => LIIsChecked);
        }
        public ReceivedRequest() { }
        public ReceivedRequest(string userReq, DateTime userStatus)
        {
            RequestersName = userReq;
            RequestersStatus = userStatus;
        }
    }


}
