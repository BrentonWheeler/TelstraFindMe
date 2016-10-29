using TelstraApp.Core.Interfaces;
using MvvmCross.Core.ViewModels;
using System;

using Android.OS;
using System.Threading.Tasks;
using Java.Lang;
using Android.Util;

namespace TelstraApp.Core.ViewModels
{

    //Author: Michael Kath (n9293833)
    public class FirstViewModel 
        : MvxViewModel
    {
        private RequestsViewModel _requests;
        private FindViewModel _find;
        private ResponseViewModel _response;
        private string cuser;
        private IDialogService dialog;
        private IUserDatabase locationsDatabase;



        public FirstViewModel(IDialogService dialog, IUserDatabase locationsDatabase)
        {
            
            this.dialog = dialog;
            this.locationsDatabase = locationsDatabase;

        }

        public string Current_User
        {
            get { return cuser; }
            set
            {
                cuser = value;
                RaisePropertyChanged(() => Current_User);
            }
        }

        public class CurrentUser
        {
            public string currentUser { get; set; }
        }

        public void Init(CurrentUser theUser)
        {
            Current_User = theUser.currentUser;
            Requests = new RequestsViewModel(dialog, locationsDatabase, Current_User);
            Find = new FindViewModel(dialog, locationsDatabase, Current_User);
            Response = new ResponseViewModel(dialog, locationsDatabase);
        }
        public ResponseViewModel Response
        {
            get { return _response; }
            set { _response = value; RaisePropertyChanged(() => Response); }
        }
        public RequestsViewModel Requests
        {
            get { return _requests; }
            set { _requests = value; RaisePropertyChanged(() => Requests); }
        }
        //Author: Michael Kath (n9293833)
        public FindViewModel Find
        {
            get { return _find; }
            set { _find = value; RaisePropertyChanged(() => Find); }
        }
        
    }
  
   

}

