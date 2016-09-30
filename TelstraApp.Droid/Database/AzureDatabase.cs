using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using TelstraApp.Core.Interfaces;
using System.IO;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using TelstraApp.Core.Models;

namespace TelstraApp.Droid.Database
{
    public class AzureDatabase : IAzureDB
    {
        MobileServiceClient azureDatabase;
        public MobileServiceClient GetMobileServiceClient()
        {
            CurrentPlatform.Init();

            //azureDatabase = new MobileServiceClient("http://qutmadsem22016wednesday3.azurewebsites.net/");
            azureDatabase = new MobileServiceClient("http://appindustries.azurewebsites.net/");
            InitializeLocal();
            return azureDatabase;
        }
        private void InitializeLocal()
        {
            var sqliteFilename = "UsersSQLite.db3";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal); // Documents folder
            var path = Path.Combine(documentsPath, sqliteFilename);
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }
            var store = new MobileServiceSQLiteStore(path);
            store.DefineTable<Users>();
            azureDatabase.SyncContext.InitializeAsync(store);
        }

    }
}