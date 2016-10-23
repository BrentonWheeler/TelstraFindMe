using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TelstraApp.Core.Interfaces;
using TelstraApp.Core.Models;

namespace TelstraApp.Core.ViewModels
{
    public class AdminViewModel : MvxViewModel
    {
        private string currentUser;
        public void Init ()
        {
            //this.currentUser = currentUser;
        }

        IUserDatabase DB;
       
        public AdminViewModel(IDialogService dialog, IUserDatabase locationsDatabase)
        {
            DB = locationsDatabase;
           
            
            
            AddEmployee = new MvxCommand(async () =>
            {
                var employees = await DB.GetEmployees(inputEmployee, "admin");
                bool found = false;
                foreach (var emp in employees)
                {
                    if (InputEmployee == emp.UserName)
                    {
                        await dialog.Show("Employee already exists", "Unable to add");
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    await dialog.Show("Employee Added", "Added");
                    Employees employe = new Employees(InputEmployee);
                    await DB.InsertEmployee(employe);
                }
                
            });
        }
        public ICommand AddEmployee { get; set;}




        private string inputEmployee;
        public string InputEmployee
        {
            get { return inputEmployee; }
            set {SetProperty(ref inputEmployee, value); }
        }
    }
}
