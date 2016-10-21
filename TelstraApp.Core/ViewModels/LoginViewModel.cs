using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TelstraApp.Core.ViewModels
{
    public class LoginViewModel
    : MvxViewModel
    {
        public ICommand GotoUser1
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<FirstViewModel>(new FirstViewModel.CurrentUser { currentUser = "User1" }));

            }
        }
        public ICommand GotoUser2
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<FirstViewModel>(new FirstViewModel.CurrentUser { currentUser = "User2" }));

            }
        }
        public ICommand GotoAdmin
        {
            get
            {
                return new MvxCommand(() => ShowViewModel<FirstViewModel>(new FirstViewModel.CurrentUser { currentUser = "Admin" }));

            }
        }

        public LoginViewModel()
        {
        }
    }
}
