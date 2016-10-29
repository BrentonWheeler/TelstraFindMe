<<<<<<< HEAD
﻿using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
=======
﻿using System;
>>>>>>> bb5088b70613e469a26618a4db86bc39fb83aca1

namespace TelstraApp.Core.ViewModels
{
    public class ReceivedRequest : MvxViewModel
    {
        private Boolean groupCB = false;
        public void changeCheckbox(bool check)
        {
            groupCB = true;
            _liisChecked = check;
            LIIsChecked = _liisChecked;
        }

        private Boolean _liisChecked = false;
        public bool LIIsChecked
        {
            get { return _liisChecked; }
            set
            {
                if (groupCB)
                {
                    RaisePropertyChanged(() => LIIsChecked);
                    groupCB = false;
                } else
                {
                    _liisChecked = !_liisChecked;
                    RaisePropertyChanged(() => LIIsChecked);
                }
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

        public ReceivedRequest() { }
        public ReceivedRequest(string userReq, DateTime userStatus)
        {
            RequestersName = userReq;
            RequestersStatus = userStatus;
        }
    }


}
