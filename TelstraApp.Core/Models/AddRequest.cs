using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelstraApp.Core.ViewModels
{
    public class AddRequest : MvxViewModel
    {
        private string userNameReq;
        private string userStatus;
        private int rbg_red;
        private int rbg_green;
        private int rbg_blue;
        public string UserNameReq
        {
            get { UpdateColor();
                return userNameReq; }

            set {
                SetProperty(ref userNameReq, value);
                //UpdateColor();
            }
        }
        public string UserStatus
        {
            get
            {
                //UpdateColor();
                return userStatus;
            }
            set {
                    SetProperty(ref userStatus, value);
                }
            }
        private MvxColor _color;

        public MvxColor CurrentColor
        {
            get {

                return _color; }
            set
            {
                _color = value;

                RaisePropertyChanged(() => CurrentColor);

            }
        }


        private void UpdateColor()
        {
            rbg_red = 0;
            rbg_green = 128;
            rbg_blue = 0;
            if (UserStatus == "Has not responded")
            {
                rbg_red = 195;
                rbg_green = 195;
                rbg_blue = 195;
            } 
            CurrentColor = new MvxColor(rbg_red, rbg_green, rbg_blue);
        }



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
            UserStatus = ReqStatus(userStatus);
            UserNameReq = userReq;
            RaisePropertyChanged(() => UserStatus);
            RaisePropertyChanged(() => UserNameReq);
        }
        public AddRequest(string userReq)
        {
            userNameReq = userReq;
            UserStatus = "Has not responded";
            RaisePropertyChanged(() => UserNameReq);
        }
    }


}
