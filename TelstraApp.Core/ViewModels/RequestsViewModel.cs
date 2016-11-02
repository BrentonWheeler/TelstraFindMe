﻿using MvvmCross.Core.ViewModels;
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

        public void refreshList()
        {
            RaisePropertyChanged(() => ListReceivedReq);
        }
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
        public ICommand LogoutReq { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public delegate void MyEventAction(string msg);
        public event MyEventAction ToastNotifcation;

        public async void getUserFromCB(string currentUser, List<ReceivedRequest> selectedRequests)
        {
            List<Users> users = await UsersDatabase.GetReqUser(currentUser, selectedRequests);
            Users sentUser = users[0];
            sentUser.Rank = 1;
            for (int i = 1;i<users.Count();i++)
            {
                sentUser.ReqFrom = sentUser.ReqFrom + "|" + users[i].ReqFrom;
                sentUser.Rank = i;
            }
            sentUser.ReqTo = currentUser;
            //ResponseData.AddRange(users);
            if (!responseLock)
            {
                switchResponseLock(false);
                ShowViewModel<ResponseViewModel>(sentUser);

            }

            //ShowViewModel<ResponseViewModel>(test);
        }

        public void switchResponseLock(bool switchTo)
        {
            responseLock = switchTo;
        }

        bool responseLock = false;
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
                if (ListOfSelectedCBs.Count > 0)
                {
                    getUserFromCB(currentUser, ListOfSelectedCBs);
                }
                
            });
            DeleteCommand = new MvxCommand(async () =>
            {
                if (await dialog.Show("Would you like to delete the selected request(s)?", "Delete Request", "Delete", "Cancel"))
                {
                    foreach (var item in ListReceivedReq)
                    {
                        if (item.LIIsChecked)
                        {
                            ToastNotifcation("Deleting request");
                            await UsersDatabase.DeleteRequest(currentUser, item.RequestersName);
                            //ListOutStandingReq.Remove(selectedUser);
                            ToastNotifcation("Request deleted");
                            //RaisePropertyChanged(() => ListOutStandingReq);
                        }
                    }
                    await checkDBLock();
                }
            });
            LogoutReq = new MvxCommand(() =>
            {
                ShowViewModel<LoginViewModel>();
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

        bool Dblock = false;
        public async Task<bool> checkDBLock(bool fromTask = false)
        {
            if (fromTask)
            {
                if (!Dblock)
                {
                   await RetrieveRequests();                    
                }
               
            }
            else
            {
                Dblock = true;
                await RetrieveRequests();
                Dblock = false;
            }

            return false;

        }

        public async  Task<IEnumerable<Users>> getRequests()
        {
            return await this.UsersDatabase.SelectToUser(this.currentUser);
        }

        public void AddRequestsToList(IEnumerable<Users> curerntReq)
        {
            receivedReq.Clear();
            foreach (var user in curerntReq)
            {
                SendReq(new ReceivedRequest(user.ReqFrom, user.ReqTime));
            }
            RaisePropertyChanged(() => ListReceivedReq);
        }



        public async Task<bool> RetrieveRequests()
        {
            //ListReceivedReq = new ObservableCollection<ReceivedRequest>();
            var curerntReq = await this.UsersDatabase.SelectToUser(this.currentUser);
            //var test = 
            receivedReq.Clear();
            foreach (var user in curerntReq)
            {
                SendReq(new ReceivedRequest(user.ReqFrom, user.ReqTime));
            }
            //ShowViewModel<FindViewModel>();
            RaisePropertyChanged(() => ListReceivedReq);
            return true;
        }

        public void SendReq(ReceivedRequest req)
        {
            if (req.RequestersName != null && req.RequestersName.Trim() != string.Empty)
            {
                receivedReq.Add(req);
            }

        }

        // if a request item on the list is pressed (USED FOR TESTING ATM)
        

        //Author Brenton Wheeler - n9294601
        //private string[] _requestCheckBoxes = { "User 1 - 5 Minutes Ago", "User 2 - 3 Hours Ago", "User 3 - Yesterday"};


    }
}


