
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
using System.Threading.Tasks;

namespace TelstraApp.Core.ViewModels
{
    //author: Michael Kath (n9293833)
    public class FindViewModel : MvxViewModel
    {
        private ObservableCollection<AddRequest> outStandingReq;
        public delegate void MyEventAction(string msg, bool hidekeyboard);
        public event MyEventAction ToastNotifcation;

        //The request list object
        public ObservableCollection<AddRequest> ListOutStandingReq
        {
            get { return outStandingReq; }
            set { SetProperty(ref outStandingReq, value); }
        }


        public ICommand SelectReqCommand { get; private set; }


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
        public ICommand SelectSearchBox { get; private set; }
        

        //author: Michael Kath (n9293833)
        //calls the search location(employees later) based on typing 2 chars
        private string searchTerm;
        public string SearchTerm
        {
            get {
                return searchTerm; }
            set
            {
               
                SetProperty(ref searchTerm, value);
               
                if (searchTerm.Length > 2)
                {
                    SearchEmployees(searchTerm);
                    RaisePropertyChanged(() => User);

                }
                if (searchTerm.Length < 2)
                {
                    User.Clear();
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
            string[] favourites = await UsersDatabase.GetFavourites(currentUser);

            if (favourites != null && favourites[0] != "")
            {
                User.Add(new Employees("Recently searched.."));
            }

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

        public void clearSearch()
        {
            User.Clear();
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

     
            //author: Michael Kath (n9293833)
            //long press on an item on the request list will prompt them to delete
            DeleteReqCommand = new MvxCommand<AddRequest>(async selectedUser =>
          {
              selectedUser.ChangeOnDelete(false);
              if (await dialog.Show("Would you like to delete this request?", "Delete Request", "Delete", "Cancel"))
              {
                  ToastNotifcation("Deleting request", false);
                  await UsersDatabase.DeleteRequest(selectedUser.UserNameReq, currentUser);
                  //ListOutStandingReq.Remove(selectedUser);
                  await populateList();
                  ToastNotifcation("Request deleted", false);
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

            SelectSearchBox = new MvxCommand(() =>
           {
               User.Clear();
               GetFavourites();
           });

            //author: Michael Kath (n9293833)
            //If the user taps on an item on the list of requests
            SelectReqCommand = new  MvxCommand<AddRequest> ( async req =>
            {
                
                var response = await UsersDatabase.GetResponse(currentUser, req.UserNameReq);

                if (response.HasResponded)
                {
                    //if they have responded and they have provided a map location
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
                    ToastNotifcation(response.ReqTo + " has not responded yet", false);
                }
            });



        }

        //Author: Michael Kath
        //Adds requests to list from DB
        public async Task<bool> populateList()
        {
            var curerntReq = await UsersDatabase.SelectViaUser(currentUser, true);
            AddRequests(curerntReq);
            return true;
        }

        //Author: Michael Kath
        //Syncs request list with database
        public async Task<bool> RetrieveItemsFromDB()
        {
             //Pull any latest employees
            await UsersDatabase.SyncAsyncEmp(true);

            //Get current requests from local
            var curerntReq = await UsersDatabase.SelectViaUser(currentUser);

            //pull from azure
            var newRequests = await UsersDatabase.SelectViaUser(currentUser, true);

            //Compare to see if there are any new
            if (newRequests.Count() != curerntReq.Count())
            {
                return true;
            }
            else
            {
                //Otherwise compare the curernt and new lists
                var newReq = newRequests.ToList();
                var curReq = curerntReq.ToList();
                for (int i = 0; i < newRequests.Count(); i++)
                {
                    // if there is a change
                    if (newReq[i].ReqTo != curReq[i].ReqTo || newReq[i].HasResponded != curReq[i].HasResponded)
                    {
                        return true;
                    }
                }
                //all contacts are the same if it ends up here.
                return false;
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
                //Add responded Requests first
                if (user.HasResponded)
                {
                    ResReqCount++;
                    TimeFormatter TimeTimer = new TimeFormatter(user.ReqTime);
                    SendReq(new AddRequest(user.ReqTo, user.HasResponded, TimeTimer.reqTime));
                }

            }
            //then add non responded requests
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
        private async Task InsertReqDB(Employees selectedUser)
        {
            await UsersDatabase.InsertRequest(selectedUser, currentUser);
            TimeFormatter TimeTimer = new TimeFormatter();
            IEnumerable<Users> req = await UsersDatabase.SelectViaUser(currentUser, false);
         
            User.Clear();
        }




        //author: Michael Kath (n9293833)
        //Adds User to list if he doesnt exist
        public async void SelectUserFromSearch(Employees selectedUser, IDialogService dialog)
        {
            if (selectedUser.UserName != "Searching..." && selectedUser.UserName != "No matches found" && selectedUser.UserName != "Recently searched..") 
            {
                // if request doesnt already exist insert into DB.
               if (!await UsersDatabase.CheckIfExists(selectedUser, currentUser))
                {
                    ToastNotifcation("Requests Sending..", true);

                    await InsertReqDB(selectedUser);
                    ToastNotifcation("Requests Sent", false);
                }
                else
                {
                    // if request does already exist then prompt user.
                    if (await dialog.Show("You have already have sent a request to this user", "Request Exists", "Send Another", "Go Back"))
                    {
                        //If "Send Another" removes old request from DB
                         var success = await UsersDatabase.DeleteRequest(selectedUser.UserName, currentUser);
                        if (success == 0)
                        {
                            //insert new request
                            ToastNotifcation("Requests Sending..", true);
                            await InsertReqDB(selectedUser);
                            ToastNotifcation("Requests Sent", false);
                            //RetrieveRequests();
                        }
                        else
                        {
                            // if deleting old request wasnt successful.
                            ToastNotifcation("Error when removing preivous request", true);
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
