using TelstraApp.Core.Interfaces;
using MvvmCross.Core.ViewModels;

namespace TelstraApp.Core.ViewModels
{

    //Author: Michael Kath (n9293833)
    public class FirstViewModel 
        : MvxViewModel
    {
        private RequestsViewModel _requests;
        private FindViewModel _find;
        private string cuser;
        private IDialogService dialog;
        private IUsersDatabase locationsDatabase;

        public FirstViewModel(ISqlite sqlite, IDialogService dialog, IUsersDatabase locationsDatabase)
        {
            Requests = new RequestsViewModel();
            this.dialog = dialog;
            this.locationsDatabase = locationsDatabase;

           
            //User = user;

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
            Find = new FindViewModel(dialog, locationsDatabase, Current_User);
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
  
    public class RequestsViewModel : MvxViewModel
    {
        private string _foo = "Requests page. Click the 'find' tab at the bottom to view Michael Kath's prototype";

        public string Foo {
            get { return _foo; }
            set { _foo = value; RaisePropertyChanged(() => Foo); }
                
        }


    }
   

}

