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

        public ICommand SelectUnitCommand { get; private set; }
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
        private ISqlite sqlite;
        //private ILocationsDatabase Users;
        private readonly IUsersDatabase UsersDatabase;
        private string currentUser = "User10";

        //author: Michael Kath (n9293833)
        public FindViewModel(ISqlite sqlite, IDialogService dialog, IUsersDatabase locationsDatabase)
        {

           
            //gets the list of locations binding
            User = new ObservableCollection<LocationAutoCompleteResult>();
            //this.database = new LocationsDatabase(sqlite);
            this.sqlite = sqlite;
            this.UsersDatabase = locationsDatabase;

            ListOutStandingReq = new ObservableCollection<AddRequest>();
            ListPendingReq = new ObservableCollection<AddRequest>();
            RetrieveAllRequests();

            SelectLocationCommand = new MvxCommand<LocationAutoCompleteResult>(selectedLocation =>
            {
                //SendReq(new AddRequest(req.LocalizedName));
                SelectLocation(selectedLocation, currentUser, dialog);
                User = new ObservableCollection<LocationAutoCompleteResult>();
                SearchTerm = string.Empty;
                RetrieveRequests();
                RaisePropertyChanged(() => SearchTerm);
                //RaisePropertyChanged(() => ListOutStandingReq);
            });

            SelectUnitCommand = new MvxCommand<AddRequest>( req =>
            {
                Bar = "Debug: select" + req.UserNameReq;


                // Users = GetLocations();
                 /*  foreach (var user in Users)
                   {
                       if (req.UserNameReq == user.LocalizedName)
                       {
                           database.DeleteLocation(user.Id);
                       }
                   } */
                   

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

        public async void SelectLocation(LocationAutoCompleteResult selectedLocation, string currentUser, IDialogService dialog)
        {
            //var azuredatabase = Mvx.Resolve<IAzureDatabase>().GetMobileServiceClient();
            //var database = new LocationsDatabase(sqlite);
           // database = locationsDatabase;
            if (!await UsersDatabase.CheckIfExists(selectedLocation, currentUser))
            {
                //database.InsertLocation(selectedLocation);
                await UsersDatabase.InsertLocation(selectedLocation, currentUser);
                RetrievePendingRequests();
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
