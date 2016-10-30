using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TelstraApp.Core.Interfaces;
using TelstraApp.Core.Models;

namespace TelstraApp.Core.ViewModels
{
    public class RequestsViewModel : MvxViewModel
    {
        private Boolean _isChecked = false;
        public List<Users> ResponseData = new List<Users>();
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                _isChecked = !_isChecked;
                RaisePropertyChanged(() => IsChecked);
            }
        }
       
        private readonly IUserDatabase UsersDatabase;
        private string currentUser;
        private IDialogService dialog;
        public ICommand SelectAll { get; private set; }
        public ICommand ResponseCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public async void getUserFromCB(string currentUser, List<ReceivedRequest> selectedRequests)
        {
            List<Users> users = await UsersDatabase.GetReqUser(currentUser, selectedRequests);
            Users sentUser = users[0];
            
            for(int i = 1;i<users.Count();i++)
            {
                sentUser.ReqFrom = sentUser.ReqFrom + "|" + users[i].ReqFrom;
                sentUser.ReqTo = currentUser;
                sentUser.Rank = i;
            }
            //ResponseData.AddRange(users);
            ShowViewModel<ResponseViewModel>(sentUser);
            //ShowViewModel<ResponseViewModel>(test);
        }
        public RequestsViewModel(IDialogService dialog, IUserDatabase locationsDatabase, string currentUser)
        {

            this.dialog = dialog;
            this.currentUser = currentUser;
            this.UsersDatabase = locationsDatabase;

            ListReceivedReq = new ObservableCollection<ReceivedRequest>();

            //RetrieveRequests();
            //dialog messages. Stil yet to be implemented properly
            this.dialog = dialog;
            //database to use azureDB functions
            UsersDatabase = locationsDatabase;


            SelectAll = new MvxCommand(() =>
            {  
                RaisePropertyChanged(() => IsChecked);
                foreach(var item in ListReceivedReq)
                {

                    item.changeCheckbox(IsChecked);
                    
                }
            });

            ResponseCommand = new MvxCommand(() =>
            {

                ListOfSelectedUsers = new List<Users>();
                ListOfSelectedCBs = new List<ReceivedRequest>();
                foreach (var item in ListReceivedReq)
                {
                    if (item.LIIsChecked)
                    {
                        ListOfSelectedCBs.Add(item);
                    }
                }
                getUserFromCB(currentUser, ListOfSelectedCBs);
                
            });
            DeleteCommand = new MvxCommand(async () =>
            {
                if (await dialog.Show("Would you like to delete the selected request(s)?", "Delete Request", "Delete", "Cancel"))
                {
                    foreach (var item in ListReceivedReq)
                    {
                        if (item.LIIsChecked)
                        {
                            //ToastNotifcation("Deleting request", false);
                            await UsersDatabase.DeleteRequest(currentUser, item.RequestersName);
                            //ListOutStandingReq.Remove(selectedUser);
                            //ToastNotifcation("Request deleted", false);
                            //RaisePropertyChanged(() => ListOutStandingReq);
                        }
                    }
                    RetrieveRequests();
                }
            });
        }

        

        

        private ObservableCollection<ReceivedRequest> receivedReq;
        private List<ReceivedRequest> ListOfSelectedCBs;
        public List<Users> ListOfSelectedUsers;
        public ObservableCollection<ReceivedRequest> ListReceivedReq
        {
            get { return receivedReq; }
            set { SetProperty(ref receivedReq, value); }
        }

        public async 
        Task
RetrieveRequests()
        {
            ListReceivedReq = new ObservableCollection<ReceivedRequest>();

            var curerntReq = await this.UsersDatabase.SelectToUser(this.currentUser);
            //var test = 

            foreach (var user in curerntReq)
            {
                SendReq(new ReceivedRequest(user.ReqFrom, user.ReqTime));
            }
            //ShowViewModel<FindViewModel>();
            RaisePropertyChanged(() => ListReceivedReq);

        }

        public void SendReq(ReceivedRequest req)
        {
            if (req.RequestersName != null && req.RequestersName.Trim() != string.Empty)
            {
                ListReceivedReq.Add(req);
            }

        }

        // if a request item on the list is pressed (USED FOR TESTING ATM)
        

        //Author Brenton Wheeler - n9294601
        //private string[] _requestCheckBoxes = { "User 1 - 5 Minutes Ago", "User 2 - 3 Hours Ago", "User 3 - Yesterday"};


    }
}


