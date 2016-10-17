using TelstraApp.Core.Interfaces;
using MvvmCross.Core.ViewModels;
using System;

using Android.OS;
using System.Threading.Tasks;

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

        private DateTime cTime;
        private DateTime runTime;


       
        public FirstViewModel(IDialogService dialog, IUserDatabase locationsDatabase)
        {
            
            this.dialog = dialog;
            this.locationsDatabase = locationsDatabase;

            //GetButton.Click += async (sender, e) => {



                // call your method to check for notifications here

                // Returning true means you want to repeat this timer



                /*           Task<int> SyncData()
                           (
                               ()=>

                           {
                               currentTimer = DateTime.Now;
                               if (currentTimer >= syncTimer)
                               {
                                   string hello = "helo";
                                   syncTimer = DateTime.Now.AddSeconds(30);
                               }

                           }

                      ));*/

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
            Func<Task> test = async () =>
             {
                 cTime = DateTime.Now;
                 runTime = DateTime.Now.AddSeconds(20);

                 while (true)
                 {
                     cTime = DateTime.Now;
                     if (cTime >= runTime)
                     {
                         await locationsDatabase.RunSync(Current_User);
                         runTime = DateTime.Now.AddSeconds(20);
                     }
                 }
             };
            Requests = new RequestsViewModel(dialog, locationsDatabase, Current_User);
            Find = new FindViewModel(dialog, locationsDatabase, Current_User);
            Response = new ResponseViewModel(dialog, locationsDatabase, Current_User);
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

