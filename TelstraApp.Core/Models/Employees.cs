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
            UserName = "";
            Favourites = "";
        }

        public Employees(string userName)
        {
            this.UserName = userName;
            Favourites = "";
        }

        public string Id { get; set; }
        public string UserName { get; set; }
        public string Favourites { get; set; }

        public void UpdateFavourites(string favourite)
        {

            if (this.Favourites == null)
            {
                this.Favourites = favourite;
            }
            else
            {
                string[] favs = this.Favourites.Split(';');
                bool found = false;
                for (var i = 0; i < favs.Length; i++)
                {
                    if (favs[i] == favourite)
                    {
                        found = true;
                        break;
                    }
                }
 
                if (favs.Length > 2 && !found)
                {
                    this.Favourites = favourite + ";" + favs[0] + ";" + favs[1];
                }
                else if (!found)
                {
                    this.Favourites = favourite + ";" + this.Favourites;
                }
            }
            //this.Favourites = "";


        }
        public string RetrieveFavourites()
        {
            if (this.Favourites == null){
                this.Favourites = "";
            }
            return this.Favourites;
            


        }

    }
}
