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
    //author: Michael Kath (n9293833)
    public class FindViewModel : MvxViewModel
    {
        private string _bar = "Debug menu:";

        //Debug display TODO remove later
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

        //calls the search location(employees later) based on typing 3 chars
        public string SearchTerm
        {
            get { return searchTerm; }
            set
            {
                SetProperty(ref searchTerm, value);
                if (searchTerm.Length > 3)
                {
                    SearchLocations(searchTerm);
                    RaisePropertyChanged(() => User);

                }
            }
        }

        //private ObservableCollection<LocationAutoCompleteResult> user;
        private ObservableCollection<Employees> user;

        public ObservableCollection<Employees> User
        {
            get { return user; }
            set { SetProperty(ref user, value); }
        }
      /*  public ObservableCollection<LocationAutoCompleteResult> User
        {
            get { return user; }
            set { SetProperty(ref user, value); }
        } */
        public ICommand SelectLocationCommand { get; private set; }

        //author: Michael Kath (n9293833)
        //Currently searches the weather locations. Will be used to implement searching employees

        public async void SearchLocations(string searchTerm)
        {
            //TODO remove this to allow searching via user
            //Employees emp = new Employees("test333");

            //SelectUserFromSearch(emp, dialog);



            User.Clear();
            //WeatherService weatherService = new WeatherService();
            //AddResponse(req, currentUser);

            //var locationResults = await weatherService.GetLocations(searchTerm);
            //var bestLocationResults = locationResults.Where(location => location.Rank > 80);
            // var result = await UsersDatabase.GetEmployees(currentUser);
            // WeatherService weatherService = new WeatherService();
            // var locationResults = await weatherService.GetLocations(searchTerm);
            // var bestLocationResults = locationResults.Where(location => location.Rank > 80);
            User.Add(new Employees("Searching..."));
            var result = await UsersDatabase.GetEmployees(searchTerm, currentUser);
            User.Clear();
         
            //var bestLocationResults = locationResults.Where(location => location.Rank > 80);
            foreach (var item in result)
            {
               User.Add(item);
            }

        }
        /*   public async void SearchLocations(string searchTerm)
           {
               //TODO remove this to allow searching via user
               WeatherService weatherService = new WeatherService();
               User.Clear();
               var locationResults = await weatherService.GetLocations(searchTerm);
               var bestLocationResults = locationResults.Where(location => location.Rank > 80);
               foreach (var item in bestLocationResults)
               {
                   User.Add(item);
               }
           } */

        //Database Stuff
        private readonly IUserDatabase UsersDatabase;
        private string currentUser;
        private IDialogService dialog;

        //author: Michael Kath (n9293833)
        public FindViewModel(IDialogService dialog, IUserDatabase locationsDatabase, string currentUser)
        {
            this.dialog = dialog;
            this.currentUser = currentUser;
            User = new ObservableCollection<Employees>();
            this.UsersDatabase = locationsDatabase;

            ListOutStandingReq = new ObservableCollection<AddRequest>();
            RetrieveRequests();

    
            SelectLocationCommand = new MvxCommand<Employees>(selectedLocation =>
            {
                SelectUserFromSearch(selectedLocation, dialog);
                //User = new ObservableCollection<Employees>();
                SearchTerm = string.Empty;
                RaisePropertyChanged(() => SearchTerm);


            });

            // if a request item on the list is pressed (USED FOR TESTING ATM)
            SelectReqCommand = new  MvxCommand<AddRequest> ( req =>
            {
                Bar = "Debug: select" + req.UserNameReq;

                //Testing updates a request with a response. Will be used to display the responses
                var result = UsersDatabase.AddResponse(req, currentUser);
                RetrieveRequests();

                RaisePropertyChanged(() => Bar);
            });



        }

        //author: Michael Kath (n9293833)
        //Displays all the outstanding requests
        public async void RetrieveRequests()
        {
            ListOutStandingReq = new ObservableCollection<AddRequest>();

            var curerntReq = await UsersDatabase.SelectViaUser(currentUser);
            //var test = 

            foreach (var user in curerntReq)
            {
                SendReq(new AddRequest(user.ReqTo, user.HasResponded));
            }
            //ShowViewModel<FindViewModel>();
            RaisePropertyChanged(() => ListOutStandingReq);
            
        }

        //author: Michael Kath (n9293833)
        //Adds User to list if he doesnt exist
        public async void SelectUserFromSearch(Employees selectedUser, IDialogService dialog)
        {

            if (!await UsersDatabase.CheckIfExists(selectedUser, currentUser) && selectedUser.UserName != "Searching...")

            {
                UsersDatabase.InsertLocation(selectedUser, currentUser);
                //UsersDatabase.InsertEmployee(selectedUser);
                SendReq(new AddRequest(selectedUser.UserName));
                //ShowViewModel<FirstViewModel>();
                //FindViewModel
                User.Clear();
                Bar = "Debug:Added: ";
                RaisePropertyChanged(() => Bar);
            }
            else
            {
                Bar = "Debug:Already been added: ";
                RaisePropertyChanged(() => Bar);
            }
        }


        //author: Michael Kath (n9293833)
        public void SendReq(AddRequest req)
        {
            if (req.UserNameReq != null && req.UserNameReq.Trim() != string.Empty)
            {
                ListOutStandingReq.Add(req);
            }

        }

    }
}
