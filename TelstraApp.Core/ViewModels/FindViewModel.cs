using Android;
using TelstraApp.Core.ViewModels;
using TelstraApp.Core.Database;
using TelstraApp.Core.Interfaces;
using TelstraApp.Core.Models;
using TelstraApp.Core.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using static TelstraApp.Core.ViewModels.FirstViewModel;

namespace TelstraApp.Core.ViewModels
{
    public class FindViewModel : MvxViewModel
    {
        private string _bar = "Debug menu:";

        public string Bar
        {
            get { return _bar; }
            set
            {
                _bar = value; RaisePropertyChanged(() => Bar);
            }

        }

        private ObservableCollection<AddRequest> outStandingReq;
        public ObservableCollection<AddRequest> ListOutStandingReq
        {
            get { return outStandingReq; }
            set { SetProperty(ref outStandingReq, value); }
        }

        private ObservableCollection<AddRequest> outPendingReq;
        public ObservableCollection<AddRequest> ListPendingReq
        {
            get { return outPendingReq; }
            set { SetProperty(ref outPendingReq, value); }
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

        public ICommand SelectReqCommand { get; private set; }
        //public ICommand SelectPendingCommand { get; private set; }

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

        private ObservableCollection<LocationAutoCompleteResult> user;

        public ObservableCollection<LocationAutoCompleteResult> User
        {
            get { return user; }
            set { SetProperty(ref user, value); }
        }
        public ICommand SelectLocationCommand { get; private set; }

        public async void SearchLocations(string searchTerm)
        {
            WeatherService weatherService = new WeatherService();
            User.Clear();
            var locationResults = await weatherService.GetLocations(searchTerm);
            var bestLocationResults = locationResults.Where(location => location.Rank > 80);
            foreach (var item in bestLocationResults)
            {
                User.Add(item);
            }
        }

        //Database Stuff
        //ILocationsDatabase database;
        //private ILocationsDatabase Users;
        private readonly IUserDatabase UsersDatabase;
        private string currentUser;

        //author: Michael Kath (n9293833)
        public FindViewModel(IDialogService dialog, IUserDatabase locationsDatabase, string currentUser)
        {

            this.currentUser = currentUser;
             //gets the list of locations binding
             User = new ObservableCollection<LocationAutoCompleteResult>();
            //this.database = new LocationsDatabase(sqlite);
           // this.sqlite = sqlite;
            this.UsersDatabase = locationsDatabase;

            ListOutStandingReq = new ObservableCollection<AddRequest>();
            //ListPendingReq = new ObservableCollection<AddRequest>();
            RetrieveRequests();

            SelectLocationCommand = new MvxCommand<LocationAutoCompleteResult>(selectedLocation =>
            {
                //SendReq(new AddRequest(req.LocalizedName));
                SelectLocation(selectedLocation, dialog);
                User = new ObservableCollection<LocationAutoCompleteResult>();
                SearchTerm = string.Empty;
                RetrieveRequests();


                RaisePropertyChanged(() => SearchTerm);
             
                //RaisePropertyChanged(() => ListOutStandingReq);
            });

            SelectReqCommand = new  MvxCommand<AddRequest> (async req =>
            {
                Bar = "Debug: select" + req.UserNameReq;

                var curerntReq = await UsersDatabase.SelectViaUser(currentUser);

                foreach (var user in curerntReq)
                   {
                       if (req.UserNameReq == user.LocalizedName)
                       {
                        if (!user.HasResponded)
                        {
                            ShowViewModel<FindViewModel>();
                            //TODO go to viewResponse page
                        }
                       }
                   }
                   

                RetrieveRequests();

                RaisePropertyChanged(() => Bar);
            });



        }



        public void RetrieveAllRequests()
        {
            RetrieveRequests();
            RetrievePendingRequests();
        }

        public async void RetrieveRequests()
        {
            ListOutStandingReq = new ObservableCollection<AddRequest>();
            //var locations = database.GetLocations();
            //var allUsers = await UsersDatabase.GetLocations();

            var curerntReq = await UsersDatabase.SelectViaUser(currentUser);

            foreach (var user in curerntReq)
            {
                SendReq(new AddRequest(user.ReqTo, user.HasResponded));
            }
            //ShowViewModel<FindViewModel>();
            RaisePropertyChanged(() => ListOutStandingReq);
            
        }

        public void RetrievePendingRequests()
        {
            ListPendingReq = new ObservableCollection<AddRequest>();
            //var locations = database.GetLocations();
            var Users1 = UsersDatabase.GetLocations();
          /*  foreach (var user in Users)
            {
                AddReqToList(new AddRequest(user.LocalizedName));
            }
            RaisePropertyChanged(() => ListPendingReq);
            */
        }

        public async void SelectLocation(LocationAutoCompleteResult selectedLocation, IDialogService dialog)
        {
            //var azuredatabase = Mvx.Resolve<IAzureDatabase>().GetMobileServiceClient();
            //var database = new LocationsDatabase(sqlite);
           // database = locationsDatabase;
            if (!await UsersDatabase.CheckIfExists(selectedLocation, currentUser))
            {
                //database.InsertLocation(selectedLocation);
                await UsersDatabase.InsertLocation(selectedLocation, currentUser);
                RetrieveRequests();
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
                
                //ListPendingReq.Add(req);
            }
            else
            {
                UserNameReq = req.UserNameReq;
                ReqStatus = req.UserStatus;
            }

        }
 /*       public void AddReqToList(AddRequest req)
        {
            if (req.UserNameReq != null && req.UserNameReq.Trim() != string.Empty)
            {
                //ListOutStandingReq.Add(req);
                ListPendingReq.Add(req);
            }
            else
            {

                UserNameReq = req.UserNameReq;
                ReqStatus = req.UserStatus;
            }

        }
        */

    }
}
