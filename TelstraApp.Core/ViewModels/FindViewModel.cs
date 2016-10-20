
using TelstraApp.Core.Interfaces;
using TelstraApp.Core.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using MvvmCross.Core.ViewModels;


namespace TelstraApp.Core.ViewModels
{
    //author: Michael Kath (n9293833)
    public class FindViewModel : MvxViewModel
    {
        private ObservableCollection<AddRequest> outStandingReq;
        public ObservableCollection<AddRequest> ListOutStandingReq
        {
            get { return outStandingReq; }
            set { SetProperty(ref outStandingReq, value); }
        }

        public ICommand ButtonCommand { get; private set; }

        public ICommand SelectReqCommand { get; private set; }

        private string searchTerm;
        private bool startedSearch = false;

        //calls the search location(employees later) based on typing 3 chars
        public string SearchTerm
        {
            get { return searchTerm; }
            set
            {
                SetProperty(ref searchTerm, value);
                if (searchTerm.Length == 0 && !startedSearch)
                {
                    //broken
                    //GetFavourites();
                }

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

        private async void GetFavourites()
        {
            //await UsersDatabase.InsertEmployee(new Employees("User1"));
           // await UsersDatabase.InsertEmployee(new Employees("User2"));
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



        //Database Stuff
        private readonly IUserDatabase UsersDatabase;
        private string currentUser;
        private IDialogService dialog;
        private int ResReqCount = 0;

        public string getCurrentUser()
        {
            return currentUser;
        }

        //author: Michael Kath (n9293833)
        public FindViewModel(IDialogService dialog, IUserDatabase locationsDatabase, string currentUser)
        {
            this.dialog = dialog;
            this.currentUser = currentUser;
            User = new ObservableCollection<Employees>();
            this.UsersDatabase = locationsDatabase;

            ListOutStandingReq = new ObservableCollection<AddRequest>();
            RetrieveRequests();
            RetrieveEmployees();
            
            


            SelectLocationCommand = new MvxCommand<Employees>(selectedLocation =>
            {
                SelectUserFromSearch(selectedLocation, dialog);
                SearchTerm = string.Empty;
                RaisePropertyChanged(() => SearchTerm);

            });

            // if a request item on the list is pressed (USED FOR TESTING ATM)
            SelectReqCommand = new  MvxCommand<AddRequest> ( async req =>
            {
                var response = await UsersDatabase.GetResponse(currentUser, req.UserNameReq);

                if (response.RespLocationLat != 0 && response.RespLocationLng != 0)
                {
                    ShowViewModel<RequestResponseViewModel>(response);
                }

                

                // UsersDatabase.DeleteRequest(req.UserNameReq, currentUser);
                // RetrieveRequests();
            });



        }

        public async void RetrieveEmployees()
        {
            await UsersDatabase.SyncAsyncEmp(true);
        }

        //author: Michael Kath (n9293833)
        //Displays all the outstanding requests



        public async void RetrieveRequests()
        {
            ListOutStandingReq = new ObservableCollection<AddRequest>();

            //Get current find Requests
            var curerntReq = await UsersDatabase.SelectViaUser(currentUser);
            AddRequests(curerntReq);

            //meanwhile push from the database and check to see if they have changed
            var newRequests = await UsersDatabase.SelectViaUser(currentUser, true);

            //if the counts are different then there must be a database change
            if (newRequests.Count() != curerntReq.Count())
            {
                //update the list
                AddRequests(newRequests);
            }
            else
            {
                var newReq = newRequests.ToList();
                var curReq = newRequests.ToList();
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
            }
            
        }

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
                }
                else
                {
                    if (await dialog.Show("You have already added a request", "Request Exists", "Send Another", "Go Back"))
                    {
                        await UsersDatabase.DeleteRequest(selectedUser.UserName, currentUser);
                        InsertReqDB(selectedUser);
                        RetrieveRequests();
                    }
                }
            }
        }


        //author: Michael Kath (n9293833)
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
