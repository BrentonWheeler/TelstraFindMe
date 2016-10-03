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
            this.userName = userName;
        }

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string userName { get; set; }



    }
}
