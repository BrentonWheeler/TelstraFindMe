using Android.App;
using Android.Graphics;
using Android.Views;
using Android.Views.InputMethods;
using MvvmCross.Core.ViewModels;
using MvvmCrossDemo.Core.Database;
using MvvmCrossDemo.Core.Interfaces;
using MvvmCrossDemo.Core.Models;
using MvvmCrossDemo.Core.Services;
using System.Collections.Generic;
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

        public FirstViewModel(ISqlite sqlite, IDialogService dialog)
        {
            Requests = new RequestsViewModel();
            Find = new FindViewModel(sqlite, dialog);

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

        //Database Stuff
        LocationsDatabase database;
        private ISqlite sqlite;
        private IEnumerable<LocationAutoCompleteResult> Users; 

        //author: Michael Kath (n9293833)
        public FindViewModel(ISqlite sqlite, IDialogService dialog)
        {

        //gets the list of locations binding
            Locations = new ObservableCollection<LocationAutoCompleteResult>();
            this.database = new LocationsDatabase(sqlite);
            this.sqlite = sqlite;
            this.Users = database.GetLocations();

            ListOutStandingReq = new ObservableCollection<AddRequest>();
            GetUsers();

            SelectLocationCommand = new MvxCommand<LocationAutoCompleteResult>(selectedLocation =>
            {
                //SendReq(new AddRequest(req.LocalizedName));
                SelectLocation(selectedLocation, dialog);
                Locations = new ObservableCollection<LocationAutoCompleteResult>();
                SearchTerm = string.Empty;
                RaisePropertyChanged(() => SearchTerm);
                RaisePropertyChanged(() => ListOutStandingReq);
            });

            SelectUnitCommand = new MvxCommand<AddRequest>(req =>
            {
               Bar = "Debug: select" + req.UserNameReq;
               foreach (var user in Users)
                {
                    if (req.UserNameReq == user.LocalizedName)
                    {
                        database.DeleteLocation(user.Id);
                    }
                }

                GetUsers();

                RaisePropertyChanged(() => Bar);
            });
        }

        public void GetUsers()
        {
            ListOutStandingReq = new ObservableCollection<AddRequest>();
            //var locations = database.GetLocations();
            Users = database.GetLocations();
            foreach (var user in Users)
            {
                SendReq(new AddRequest(user.LocalizedName));
            }
            RaisePropertyChanged(() => ListOutStandingReq);
            //return ListOutStandingReq;
        }

        public void SelectLocation(LocationAutoCompleteResult selectedLocation, IDialogService dialog)
        {
            //var azuredatabase = Mvx.Resolve<IAzureDatabase>().GetMobileServiceClient();
            var database = new LocationsDatabase(sqlite);

            if (!database.CheckIfExists(selectedLocation))
            {
                database.InsertLocation(selectedLocation);
                GetUsers();
                //SendReq(new AddRequest(selectedLocation.LocalizedName));

                Bar = "Debug:Added: ";
                RaisePropertyChanged(() => Bar);
                /* await azuredatabase.GetTable<Location>().InsertAsync(new Location
                 {
                     Key = selectedLocation.Key,
                     LocalizedName = selectedLocation.LocalizedName,
                     Rank = selectedLocation.Rank
                 }); */
                //Close(this);
            }
            else
            {
                Bar = "Debug:Already been added: ";
                RaisePropertyChanged(() => Bar);

                /* if (await dialog.Show("This location has already been added", "Location Exists", "Keep Searching", "Go Back"))
                 {
                     SearchTerm = string.Empty;
                     Locations.Clear();
                 }
                 else
                 {
                    // Close(this);
                 }  */
            }
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

