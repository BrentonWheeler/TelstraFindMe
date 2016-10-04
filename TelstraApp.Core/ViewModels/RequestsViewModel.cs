using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelstraApp.Core.Interfaces;

namespace TelstraApp.Core.ViewModels
{
    public class RequestsViewModel : MvxViewModel
    {

        IDialogService dialog;
        IUserDatabase locationsDatabase;
        public RequestsViewModel(IDialogService dialog, IUserDatabase locationsDatabase, string currentUser)
        {
            //dialog messages. Stil yet to be implemented properly
            this.dialog = dialog;
            //database to use azureDB functions
            this.locationsDatabase = locationsDatabase;

        }

            //Author Brenton Wheeler - n9294601
            //private string[] _requestCheckBoxes = { "User 1 - 5 Minutes Ago", "User 2 - 3 Hours Ago", "User 3 - Yesterday"};


    }
}
