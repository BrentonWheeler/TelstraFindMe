using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite.Net;
using TelstraApp.Core.Interfaces;
using TelstraApp.Core.Models;
using MvvmCross.Platform;
using System.Threading.Tasks;

namespace TelstraApp.Core.Database
{

        public class UsersDatabase : IUsersDatabase
    {
            private SQLiteConnection database;
            public UsersDatabase()
            {
                var sqlite = Mvx.Resolve<ISqlite>();
                database = sqlite.GetConnection();
                database.CreateTable<Users>();
            }

            public async Task<IEnumerable<Users>> GetLocations()
            {
                return database.Table<Users>().ToList();
            }

            public async Task<int> DeleteLocation(object id)
            {
                return database.Delete<Users>(Convert.ToInt16(id));
            }

            public async Task<int> InsertLocation(Users location, string currentUser)
            {
                var num = database.Insert(location);
                database.Commit();
                return num;
            }

            public async Task<bool> CheckIfExists(Users location, string currentUser)
            {
                var exists = database.Table<Users>()
                    .Any(x => x.LocalizedName == location.LocalizedName
                    && x.ReqFrom == currentUser);
                return exists;

            }

        public async Task<IEnumerable<Users>> SelectViaUser(string currentUser)
        {
            var Users = from users in database.Table<Users>()
                        orderby users.ReqTime descending
                        where users.ReqFrom == currentUser
                        select users;
            return Users.ToList();
        }

        public async Task<int> InsertLocation(LocationAutoCompleteResult location, string currentUser)
            {
                return await InsertLocation(new Users(location, currentUser), currentUser);
            }

            public async Task<bool> CheckIfExists(LocationAutoCompleteResult location, string currentUser)
            {
                return await CheckIfExists(new Users(location, currentUser), currentUser);
            }
        }
    }