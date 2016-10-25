
using TelstraApp.Core.Interfaces;
using TelstraApp.Core.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;
using Android.Widget;
using Android.Content;
using System;

namespace TelstraApp.Core.ViewModels
{
    //author: Michael Kath (n9293833)
    public class FindViewModel : MvxViewModel
    {
        private ObservableCollection<AddRequest> outStandingReq;
        public delegate void MyEventAction(string msg, bool hidekeyboard);
        public event MyEventAction MyEvent;

            
        public ObservableCollection<AddRequest> ListOutStandingReq
        {
            get { return outStandingReq; }
            set { SetProperty(ref outStandingReq, value); }
        }

        private ObservableCollection<string> deleteStandingReq;
        public ObservableCollection<string> DelListOutStandingReq
        {
            get { return deleteStandingReq; }
            set { SetProperty(ref deleteStandingReq, value); }
        }

        public ICommand ButtonCommand { get; private set; }


        public ICommand SelectReqCommand { get; private set; }


        private string searchTerm;
        private bool startedSearch = false;

        private string userDelete;
        public string UserDelete
        {
            get
            {
                return userDelete;
            }
            set
            {
                SetProperty(ref userDelete, value);
            }

        }
        //author: Michael Kath (n9293833)
        //calls the search location(employees later) based on typing 2 chars
        public string SearchTerm
        {
            get { return searchTerm; }
            set
            {
                SetProperty(ref searchTerm, value);

                if (searchTerm.Length > 2)
                {
                    startedSearch = true;
                    SearchEmployees(searchTerm);
                    RaisePropertyChanged(() => User);

                }
                if (startedSearch && searchTerm.Length < 2)
                {
                    User.Clear();
                    startedSearch = false;
                }

                
            }
        }
        //author: Michael Kath (n9293833)
        //gets a list of users based on the search
        public async void SearchEmployees(string searchTerm)
        {
            User.Clear();
            User.Add(new Employees("Searching..."));
            var result = await UsersDatabase.GetEmployees(searchTerm, currentUser);
            User.Clear();
            if (result.Any())
            {
                foreach (var item in result)
                {
                    User.Add(item);
                }
            }
            else
            {
                User.Add(new Employees("No matches found"));
            }
        }

        //author: Michael Kath (n9293833)
        private async void GetFavourites()
        {
            var favourites = await UsersDatabase.GetFavourites(currentUser);

            foreach (var fav in favourites)
            {
                User.Add(new Employees(fav));
            }
        }

        //private ObservableCollection<LocationAutoCompleteResult> user;
        private ObservableCollection<Employees> user;

        public ObservableCollection<Employees> User
        {
            get { return user; }
            set { SetProperty(ref user, value); }
        }

        public ICommand SelectLocationCommand { get; private set; }
        public int addMessage1 { get; set; }
        public MvxCommand<AddRequest> DeleteReqCommand { get; private set; }

        private readonly IUserDatabase UsersDatabase;
        private string currentUser;
        private IDialogService dialog;
        private int ResReqCount = 0;

        public string getCurrentUser()
        {
            return currentUser;
        }

        //author: Michael Kath (n9293833)
        //The main entry point, sets up local vars to be used later
        public FindViewModel(IDialogService dialog, IUserDatabase locationsDatabase, string currentUser)
        {
            this.dialog = dialog;
            this.currentUser = currentUser;
            User = new ObservableCollection<Employees>();
            this.UsersDatabase = locationsDatabase;

            ListOutStandingReq = new ObservableCollection<AddRequest>();
            DelListOutStandingReq = new ObservableCollection<string>();



            DeleteReqCommand = new MvxCommand<AddRequest>(async selectedUser =>
          {
              selectedUser.ChangeOnDelete(false);
              if (await dialog.Show("Would you like to delete this request?", "Delete Request", "Delete", "Cancel"))
              {
                  await UsersDatabase.DeleteRequest(selectedUser.UserNameReq, currentUser);
                  ListOutStandingReq.Remove(selectedUser);
                  MyEvent("Request Deleted", false);
                  RaisePropertyChanged(() => ListOutStandingReq);

              }
              else
              {
                  selectedUser.ChangeOnDelete(true);
              }
             
          });

            SelectLocationCommand = new MvxCommand<Employees>(selectedLocation =>
            {
                SelectUserFromSearch(selectedLocation, dialog);
                SearchTerm = string.Empty;
      
                RaisePropertyChanged(() => SearchTerm);
               

            });

            //author: Michael Kath (n9293833)
            //If the user taps on an item on the list of requests
            SelectReqCommand = new  MvxCommand<AddRequest> ( async req =>
            {
                
                var response = await UsersDatabase.GetResponse(currentUser, req.UserNameReq);

                if (response.HasResponded)
                {
                    if (response.RespLocationLat != 0 && response.RespLocationLng != 0)
                    {
                        ShowViewModel<RequestResponseViewModel>(response);
                    }
                    else
                    {
                        ShowViewModel<RequestResponse1ViewModel>(response);
                    }
                }
                else
                {
                    MyEvent("User has not responded yet", false);
                }
            });



        }
        //Author: Michael Kath
        //Syncs with database
        public async void RetrieveEmployees()
        {

            /* Employees em = new Employees("User2");

             await UsersDatabase.InsertLocation(em, "User16");
             await UsersDatabase.InsertLocation(em, "User17");
             await UsersDatabase.InsertLocation(em, "User13");
             await UsersDatabase.InsertLocation(em, "User14");
             await UsersDatabase.InsertLocation(em, "User12");
             await UsersDatabase.InsertLocation(em, "User11");
             await UsersDatabase.InsertLocation(em, "User10");
             await UsersDatabase.InsertLocation(em, "User2"); */

            await UsersDatabase.SyncAsyncEmp(true);
        }

        //author: Michael Kath (n9293833)
        //Displays all the outstanding requests

        public async void RetrieveRequests()
        {
            ListOutStandingReq = new ObservableCollection<AddRequest>();
            DelListOutStandingReq = new ObservableCollection<string>();
            //Get current find Requests
            var curerntReq = await UsersDatabase.SelectViaUser(currentUser);
            AddRequests(curerntReq);

            //meanwhile push from the database and check to see if they have changed
            MyEvent("Syncing Contacts", false);
            var newRequests = await UsersDatabase.SelectViaUser(currentUser, true);

            //if the counts are different then there must be a database change
            if (newRequests.Count() != curerntReq.Count())
            {
                //update the list
                AddRequests(newRequests);
                MyEvent("Contacts Synced", false);
            }
            else
            {
                var newReq = newRequests.ToList();
                var curReq = curerntReq.ToList();
                // compare every new request received from DB against the current DB
                for (int i = 0; i < newRequests.Count(); i++)
                {
                    // if there is a change
                    if (newReq[i] != curReq[i])
                    {
                        //Update the entire list
                        AddRequests(newRequests);
                        break;
                    }
                }
               MyEvent("Contacts Synced", false);
            }
            
        }
        //author: Michael Kath (n9293833)
        //loops through all the new requests found to see if they already list on the users list
        private void AddRequests(IEnumerable<Users> newRequests)
        {
            ListOutStandingReq = new ObservableCollection<AddRequest>();
            ResReqCount = 0;
            foreach (var user in newRequests)
            {
                
                if (user.HasResponded)
                {
                    ResReqCount++;
                    TimeFormatter TimeTimer = new TimeFormatter(user.ReqTime);
                    SendReq(new AddRequest(user.ReqTo, user.HasResponded, TimeTimer.reqTime));
                }

            }

            foreach (var user in newRequests)
            {
                if (!user.HasResponded)
                {
                    TimeFormatter TimeTimer = new TimeFormatter(user.ReqTime);
                    SendReq(new AddRequest(user.ReqTo, user.HasResponded, TimeTimer.reqTime));
                }

            }

            RaisePropertyChanged(() => ListOutStandingReq);
        }
        //author: Michael Kath (n9293833)
        //Inserts into database and formats the time to be displayed on the users list
        private void InsertReqDB(Employees selectedUser)
        {
            UsersDatabase.InsertLocation(selectedUser, currentUser);
            TimeFormatter TimeTimer = new TimeFormatter();
            SendReq(new AddRequest(selectedUser.UserName, TimeTimer.reqTime), false);
            User.Clear();
        }




        //author: Michael Kath (n9293833)
        //Adds User to list if he doesnt exist
        public async void SelectUserFromSearch(Employees selectedUser, IDialogService dialog)
        {
            if (selectedUser.UserName != "Searching..." && selectedUser.UserName != "No matches found") { }
            {
               if (!await UsersDatabase.CheckIfExists(selectedUser, currentUser))
                {
                    InsertReqDB(selectedUser);
                    MyEvent("Requests Sent", true);
                }
                else
                {
                    if (await dialog.Show("You have already have sent a request to this user", "Request Exists", "Send Another", "Go Back"))
                    {
                         var success = await UsersDatabase.DeleteRequest(selectedUser.UserName, currentUser);
                        if (success == 0)
                        {
                            InsertReqDB(selectedUser);
                            MyEvent("Requests Sent", true);
                            RetrieveRequests();
                        }
                        else
                        {
                            MyEvent("Error when removing preivous request", true);
                        }

                    }
                }
            }
        }


        //author: Michael Kath (n9293833)
        //Adds the new requst to the list
        public void SendReq(AddRequest req, bool AddLast = true)
        {
            if (req.UserNameReq != null && req.UserNameReq.Trim() != string.Empty)
            {
               if (AddLast)
                {
                    ListOutStandingReq.Add(req);
                  
                }
                else
                {
                    ListOutStandingReq.Insert(ResReqCount, req);
                }
               
            }

        }

    }
}
