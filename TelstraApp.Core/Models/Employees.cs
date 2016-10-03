using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelstraApp.Core.Models
{
    public class Employees
    {
        public Employees()
        {

        }

        public Employees(string userName)
        {
            this.UserName = userName;
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string Favourites { get; set; }



    }
}
