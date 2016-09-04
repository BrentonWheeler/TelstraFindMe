using MvvmCross.Core.ViewModels;

namespace TelstraApp.Core.ViewModels
{
    

    public class FirstViewModel 
        : MvxViewModel
    {

        private RequestsViewModel _requests;
        private FindViewModel _find;
        

        public FirstViewModel()
        {
            Requests = new RequestsViewModel();
            Find = new FindViewModel();

        }

        public RequestsViewModel Requests
        {
            get { return _requests; }
            set { _requests = value; RaisePropertyChanged(() => Requests); }
        }
        public FindViewModel Find
        {
            
            get { return _find; }
            set { _find = value; RaisePropertyChanged(() => Find); }
        }
        
    }

    public class RequestsViewModel : MvxViewModel
    {
        //Author Brenton Wheeler - n9294601
        //private string[] _requestCheckBoxes = { "User 1 - 5 Minutes Ago", "User 2 - 3 Hours Ago", "User 3 - Yesterday"};

    }
    public class FindViewModel : MvxViewModel
    {
        //private string _bar = "hello bar";

        //public string Bar
        //{
            //get { return _bar; }
            //set { _bar = value; RaisePropertyChanged(() => Bar); }

        //}
    }
}

