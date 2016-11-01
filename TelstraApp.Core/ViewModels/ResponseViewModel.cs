using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Converters;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TelstraApp.Core.Database;
using TelstraApp.Core.Interfaces;
using TelstraApp.Core.Models;

namespace TelstraApp.Core.ViewModels
{
    public class ResponseViewModel : MvxViewModel
    {
        public float myLat;
        public float myLong;
        public string currentUser;
        List<Users> users;
        public string _room = "";
        public String Room
        {
            get { return _room; }
            set
            {
                _room = value;
                RaisePropertyChanged(() => Room);
            }
        }
        private String _userMsg = "";
        public String UserMsg
        {
            get { return _userMsg; }
            set
            {
                _userMsg = value;
                RaisePropertyChanged(() => UserMsg);
            }
        }
        private String _locationText = "Add Location";
        public String LocationText
        {
            get { return _locationText; }
            set
            {
                if (value != null)
                {
                    RaisePropertyChanged(() => LocationText);
                }
            }
        }
        private Boolean _meetingIsDisabled = true;
        public Boolean MeetingIsDisabled
        {
            get { return _meetingIsDisabled; }
            set
            {
                RaisePropertyChanged(() => MeetingIsDisabled);
            }
        }
        private Boolean _lunchIsDisabled = true;
        public Boolean LunchIsDisabled
        {
            get { return _lunchIsDisabled; }
            set
            {
                RaisePropertyChanged(() => LunchIsDisabled);
            }
        }
        private Boolean _homeIsDisabled = true;
        public Boolean HomeIsDisabled
        {
            get { return _homeIsDisabled; }
            set
            {
                RaisePropertyChanged(() => HomeIsDisabled);
            }
        }
        private readonly IUserDatabase UsersDatabase;
        //private string currentUser;
        public List<Users> selectedUsers;
        public ICommand LogoutRes { get; private set; }
        private IDialogService dialog;
        public ICommand AddLocation { get; private set; }
        public ICommand AtMeetingCommand { get; private set; }
        public ICommand AtHome { get; private set; }
        public ICommand AtLunch { get; private set; }
        public ICommand SendResponse { get; private set; }
        public async void Init(Users passedUsers)
        {
            string userNames = passedUsers.ReqFrom;
            string[] userNameArray;
            if (userNames.Contains("|"))
            {
                userNameArray = userNames.Split('|');
                
            }else
            {
                userNameArray = new string[1];
                userNameArray[0] = userNames;
            }
            this.currentUser = passedUsers.ReqTo;
            //query for a list of users from userNameArray's usernames
            this.users = await UsersDatabase.GetReqUserFromNameArray(currentUser, userNameArray);

        }

        public string TrimEnd(string input, string suffixToRemove)
        {
            if (input != null && suffixToRemove != null
              && input.EndsWith(suffixToRemove))
            {
                return input.Substring(0, input.Length - suffixToRemove.Length);
            }
            else return input;
        }
        public ResponseViewModel(IDialogService dialog, IUserDatabase locationsDatabase)
        {

            this.dialog = dialog;
            
            this.UsersDatabase = locationsDatabase;

            ListReceivedReq = new ObservableCollection<ReceivedRequest>();

            //dialog messages. Stil yet to be implemented properly
            this.dialog = dialog;
            //database to use azureDB functions
            UsersDatabase = locationsDatabase;

            AddLocation = new MvxCommand(() =>
            {
                if (_locationText == "Add Location")
                {
                    _locationText = "Remove Location";
                    RaisePropertyChanged(() => LocationText);
                    //remove location code
                    foreach (var user in this.users)
                    {
                        user.RespLocationLat = myLat;
                        user.RespLocationLng = myLong;
                    }
                }
                else
                {
                    _locationText = "Add Location";
                    RaisePropertyChanged(() => LocationText);
                    //add location code
                    foreach (var user in this.users)
                    {
                        user.RespLocationLat = 0;
                        user.RespLocationLng = 0;
                    }
                }
            });

            AtMeetingCommand = new MvxCommand(() =>
            {
                if (_userMsg.Contains(" (at meeting)"))
                {
                    foreach (var user in users)
                    {
                        user.RespMeeting = false;
                    }
                    _userMsg = TrimEnd(_userMsg, " (at meeting)");
                    RaisePropertyChanged(() => UserMsg);
                    _homeIsDisabled = true;
                    _lunchIsDisabled = true;
                    RaisePropertyChanged(() => HomeIsDisabled);
                    RaisePropertyChanged(() => LunchIsDisabled);
                }
                else
                {
                    foreach (var user in this.users)
                    {
                        user.RespMeeting = true;
                    }
                    _userMsg = _userMsg + " (at meeting)";
                    RaisePropertyChanged(() => UserMsg);
                    _homeIsDisabled = false;
                    _lunchIsDisabled = false;
                    RaisePropertyChanged(() => HomeIsDisabled);
                    RaisePropertyChanged(() => LunchIsDisabled);
                }
            });

            AtHome = new MvxCommand(() =>
            {
                if (_userMsg.Contains(" (at home)"))
                {
                    foreach (var user in this.users)
                    {
                        user.RespHome = false;
                    }
                    _userMsg = TrimEnd(_userMsg, " (at home)");
                    RaisePropertyChanged(() => UserMsg);
                    _meetingIsDisabled = true;
                    _lunchIsDisabled = true;
                    RaisePropertyChanged(() => MeetingIsDisabled);
                    RaisePropertyChanged(() => LunchIsDisabled);
                }
                else
                {
                    foreach (var user in this.users)
                    {
                        user.RespHome = true;
                    }
                    _userMsg = _userMsg + " (at home)";
                    RaisePropertyChanged(() => UserMsg);
                    _meetingIsDisabled = false;
                    _lunchIsDisabled = false;
                    RaisePropertyChanged(() => MeetingIsDisabled);
                    RaisePropertyChanged(() => LunchIsDisabled);
                }
            });

            AtLunch = new MvxCommand(() =>
            {
                if (_userMsg.Contains(" (at lunch)"))
                {
                    foreach (var user in this.users)
                    {
                        user.RespLunch = false;
                    }
                    _userMsg = TrimEnd(_userMsg, " (at lunch)");
                    RaisePropertyChanged(() => UserMsg);
                    _meetingIsDisabled = true;
                    _homeIsDisabled = true;
                    RaisePropertyChanged(() => MeetingIsDisabled);
                    RaisePropertyChanged(() => HomeIsDisabled);
                }
                else
                {
                    foreach (var user in this.users)
                    {
                        user.RespLunch = true;
                    }
                    _userMsg = _userMsg + " (at lunch)";
                    RaisePropertyChanged(() => UserMsg);
                    _meetingIsDisabled = false;
                    _homeIsDisabled = false;
                    RaisePropertyChanged(() => MeetingIsDisabled);
                    RaisePropertyChanged(() => HomeIsDisabled);
                }
            });

            SendResponse = new MvxCommand(async () =>
            {
                foreach (Users user in this.users)
                {
                    user.HasResponded = true;
                    user.ReqTime = DateTime.Now;
                    user.RespRoom = this.Room;
                    user.RespCurrentlyAt = this.UserMsg;
                    int result = await UsersDatabase.AddResponse(user, this.currentUser);
                }
                Close(this);
            });
            LogoutRes = new MvxCommand(() =>
            {
                ShowViewModel<LoginViewModel>();
            });
        }
       
        private ObservableCollection<ReceivedRequest> receivedReq;
        public ObservableCollection<ReceivedRequest> ListReceivedReq
        {
            get { return receivedReq; }
            set { SetProperty(ref receivedReq, value); }
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


