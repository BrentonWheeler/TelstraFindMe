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

        public void UpdateFavourites(string favourite)
        {
          

            if (!this.Favourites.Contains(favourite))
            {
                string[] favs = this.Favourites.Split(';');
                string newFavs = this.Favourites;

                if (newFavs == "" || newFavs == null)
                {
                    this.Favourites = favourite;
                }
                else if (favs.Length > 2)
                {
                    this.Favourites = favourite + ";" + favs[0] + ";" + favs[1];
                }
                else
                {
                    this.Favourites = favourite + ";" + this.Favourites;
                }
            }
            //this.Favourites = "";


        }
        public string RetrieveFavourites()
        {
            return this.Favourites;
            


        }

    }
}
