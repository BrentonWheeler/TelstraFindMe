using Android.Graphics;
using MvvmCross.Core.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace TelstraApp.Core.ViewModels
{

    //Author: Michael Kath (n9293833)
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
    //Author: Michael Kath (n9293833)
    public class FindViewModel : MvxViewModel
    {
        private string _bar = "Click a response";

        public string Bar
        {
            get { return _bar; }
            set {_bar = value; RaisePropertyChanged(() => Bar); 
            }

        }

        private ObservableCollection<AddRequest> outStandingReq;
        public ObservableCollection<AddRequest> ListOutStandingReq
        {
            get { return outStandingReq; }
            set { SetProperty(ref outStandingReq, value); }
        }
        //author: Michael Kath (n9293833)
        private string userNameReq;
        public string UserNameReq
        {
            get { return userNameReq; }
            set
            {
                if (value != null)
                {
                    SetProperty(ref userNameReq, value);
                }
            }
        }



        //author: Michael Kath (n9293833)
        private string reqStatus;
        public string ReqStatus
        {
            get { return reqStatus; }
            set
            {
                if (value != null)
                {
                    SetProperty(ref reqStatus, value);
                }
            }
        }

        public ICommand ButtonCommand { get; private set; }

        public ICommand SelectUnitCommand { get; private set; }
        //author: Michael Kath (n9293833)
        public FindViewModel()
        {

            ListOutStandingReq = new ObservableCollection<AddRequest>();

            //add dummy requests
            SendReq(new AddRequest("User12", "Has responded"));
            SendReq(new AddRequest("User10"));
            SendReq(new AddRequest("User11", "Has ignored request"));

            ButtonCommand = new MvxCommand(() =>
            {
                SendReq(new AddRequest(UserNameReq));
                RaisePropertyChanged(() => ListOutStandingReq);
            });



            SelectUnitCommand = new MvxCommand<AddRequest>(req =>
            {

               if (req.UserStatus == "Has not responded")
                {
                    Bar = "Has responded";
                 
                } else
                {

                    Bar = "Has not responded";
                }
                RaisePropertyChanged(() => Bar);
            });
        }
        //author: Michael Kath (n9293833)
        public void SendReq(AddRequest req)
        {
            if (req.UserNameReq != null && req.UserNameReq.Trim() != string.Empty)
            {
                    ListOutStandingReq.Add(req);
            }
            else
            {
                //Note this code just removes spaces from the EditText if that is all was in them
                UserNameReq = req.UserNameReq;
                ReqStatus = req.UserStatus;
            }
            
        }




    }

}

