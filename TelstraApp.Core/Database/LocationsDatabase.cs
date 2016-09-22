using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SQLite.Net;
using MvvmCrossDemo.Core.Interfaces;
using MvvmCrossDemo.Core.Models;

namespace MvvmCrossDemo.Core.Database
{
    public class LocationsDatabase
    {
        private SQLiteConnection database;
        public LocationsDatabase(ISqlite sqlite)
        {
            database = sqlite.GetConnection();
            database.CreateTable<LocationAutoCompleteResult>();
        }

        public IEnumerable<LocationAutoCompleteResult> GetLocations()
        {
            return database.Table<LocationAutoCompleteResult>().ToList();
        }

        public int DeleteLocation(int id)
        {
            return database.Delete<LocationAutoCompleteResult>(id);
        }

        public int InsertLocation(LocationAutoCompleteResult location)
        {
            var num = database.Insert(location);
            database.Commit();
            return num;
        }

        public bool CheckIfExists(LocationAutoCompleteResult location)
        {
            var exists = database.Table<LocationAutoCompleteResult>()
                .Any(x => x.LocalizedName == location.LocalizedName
                || x.Key == location.Key);
            return exists;
        }

    }
}