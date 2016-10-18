using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TelstraApp.Core.Interfaces;

namespace TelstraApp.Core.ViewModels
{
    public class RequestsViewModel : MvxViewModel
    {
        private Boolean _isChecked = true;
        public bool IsChecked
        {
            get { return _isChecked; }
            set
            {
                RaisePropertyChanged(() => IsChecked);
            }
        }

        private readonly IUserDatabase UsersDatabase;
        private string currentUser;
        private IDialogService dialog;
        public ICommand SelectAll { get; private set; }

        public RequestsViewModel(IDialogService dialog, IUserDatabase locationsDatabase, string currentUser)
        {

            this.dialog = dialog;
            this.currentUser = currentUser;
            this.UsersDatabase = locationsDatabase;

            ListReceivedReq = new ObservableCollection<ReceivedRequest>();

            RetrieveRequests();
            //dialog messages. Stil yet to be implemented properly
            this.dialog = dialog;
            //database to use azureDB functions
            UsersDatabase = locationsDatabase;


            SelectAll = new MvxCommand(() =>
            {

                _isChecked = !_isChecked;
                IsChecked = _isChecked;

                foreach (var item in ListReceivedReq)
                {
                    item.checkBox(IsChecked);
                }
                RaisePropertyChanged(() => ListReceivedReq);
                //RaisePropertyChanged(() => LIIsChecked);
            });
        }

        public ICommand ResponseCommand
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<ResponseViewModel>(new ResponseViewModel.CurrentUser { currentUser = this.currentUser }));

            }
        }

        private ObservableCollection<ReceivedRequest> receivedReq;
        public ObservableCollection<ReceivedRequest> ListReceivedReq
        {
            get { return receivedReq; }
            set { SetProperty(ref receivedReq, value); }
        }

        public async void RetrieveRequests()
        {
            ListReceivedReq = new ObservableCollection<ReceivedRequest>();

            var curerntReq = await this.UsersDatabase.SelectToUser(this.currentUser);
            //var test = 

            foreach (var user in curerntReq)
            {
                SendReq(new ReceivedRequest(user.ReqTo, user.ReqTime));
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


