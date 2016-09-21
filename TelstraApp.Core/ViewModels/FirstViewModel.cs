using Android.App;
using Android.Graphics;
using Android.Views;
using Android.Views.InputMethods;
using MvvmCross.Core.ViewModels;
using MvvmCrossDemo.Core.Models;
using MvvmCrossDemo.Core.Services;
using System.Collections.ObjectModel;
using System.Linq;
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
        private string _bar = "Debug menu:";

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

   

        private string searchTerm;

        public string SearchTerm
        {
            get { return searchTerm; }
            set
            {
                SetProperty(ref searchTerm, value);
                if (searchTerm.Length > 3)
                {
                    SearchLocations(searchTerm);
                }
            }
        }

        private ObservableCollection<LocationAutoCompleteResult> locations;

        public ObservableCollection<LocationAutoCompleteResult> Locations
        {
            get { return locations; }
            set { SetProperty(ref locations, value); }
        }
        public ICommand SelectLocationCommand { get; private set; }

        public async void SearchLocations(string searchTerm)
        {
            WeatherService weatherService = new WeatherService();
            Locations.Clear();
            var locationResults = await weatherService.GetLocations(searchTerm);
            var bestLocationResults = locationResults.Where(location => location.Rank > 80);
            foreach (var item in bestLocationResults)
            {
                Locations.Add(item);
            }
        }

        //author: Michael Kath (n9293833)
        public FindViewModel()
        {

            //Start search button

            //gets the list of locations binding
            Locations = new ObservableCollection<LocationAutoCompleteResult>();
            // if the user clicks on 1 of the items on the list
            
            //goto results later on TODO
            //SelectLocationCommand = new MvxCommand<LocationAutoCompleteResult>(selectedLocation => ShowViewModel<SecondViewModel>(selectedLocation));
            
            //Endf search

            ListOutStandingReq = new ObservableCollection<AddRequest>();

            //add dummy requests
            SendReq(new AddRequest("User12", "Has responded"));
            SendReq(new AddRequest("User10"));
            SendReq(new AddRequest("User11", "Has ignored request"));

            SelectLocationCommand = new MvxCommand<LocationAutoCompleteResult>(req =>
            {
                SendReq(new AddRequest(req.LocalizedName));
                Locations = new ObservableCollection<LocationAutoCompleteResult>();
                searchTerm = "";
                RaisePropertyChanged(() => SearchTerm);
                RaisePropertyChanged(() => ListOutStandingReq);
            });

            SelectUnitCommand = new MvxCommand<AddRequest>(req =>
            {
               Bar = "Debug: select" + req.UserNameReq;

                var copy = new ObservableCollection<AddRequest>(outStandingReq);

                foreach (var item in copy)
                {
                    if (item.UserNameReq == req.UserNameReq)
                    {
                        outStandingReq.Remove(item);
                        Bar = "Debug:Deleted request for: " + item;
                    }
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
              
                UserNameReq = req.UserNameReq;
                ReqStatus = req.UserStatus;
            }
            
        }




    }

}

