using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelstraApp.Core.Models;

namespace TelstraApp.Core.ViewModels
{
    public class ReceivedRequest : MvxViewModel
    {
        private Boolean groupCB = false;
        private int rbg_red;
        private int rbg_green;
        private int rbg_blue;
        public void changeCheckbox(bool check)
        {
            groupCB = true;
            _liisChecked = check;
            LIIsChecked = _liisChecked;
        }
        private MvxColor _color;
        public MvxColor CurrentColor
        {
            get
            {

                return _color;
            }
            set
            {
                _color = value;

                RaisePropertyChanged(() => CurrentColor);

            }
        }
        private void DeleteColor()
        {
            rbg_red = 199;
            rbg_green = 16;
            rbg_blue = 26;
            CurrentColor = new MvxColor(rbg_red, rbg_green, rbg_blue);
        }


        private void UpdateColor()
        {
            rbg_red = 0;
            rbg_green = 128;
            rbg_blue = 0;
            CurrentColor = new MvxColor(rbg_red, rbg_green, rbg_blue);
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
        public string RequestersStatus { get; set; }

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
            TimeFormatter time = new TimeFormatter(userStatus);
            RequestersStatus = time.reqTime;
        }
    }


}
