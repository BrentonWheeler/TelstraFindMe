﻿
using MvvmCross.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelstraApp.Core.Interfaces;
using TelstraApp.Core.Models;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System.Diagnostics;

namespace TelstraApp.Core.Database
{
    public class AzureDB : IUserDatabase
    {
        private MobileServiceClient azureDatabase;
        private IMobileServiceSyncTable<Users> azureSyncTable;
        public AzureDB()
        {
            azureDatabase = Mvx.Resolve<IAzureDB>().GetMobileServiceClient();
            azureSyncTable = azureDatabase.GetSyncTable<Users>();
        }

        public async Task<bool> CheckIfExists(LocationAutoCompleteResult location, string currentUser)
        {
            var exists = await CheckIfExists(new Users(location, currentUser), currentUser);
            return exists;
        }

        public async Task<bool> CheckIfExists(Users location, string currentUser)
        {
            await SyncAsync(currentUser, true);
            var locations = await azureSyncTable.Where(x => x.ReqTo == location.ReqTo && x.ReqFrom == currentUser).ToListAsync();
            return locations.Any();
        }

        public async Task<int> DeleteRequest(object id, string currentUser)
        {
            await SyncAsync(currentUser, true );
            var location = await azureSyncTable.Where(x => x.Id == id.ToString()).ToListAsync();
            if (location.Any())
            {
                await azureSyncTable.DeleteAsync(location.FirstOrDefault());
                await SyncAsync(currentUser);
                return 1;
            }
            else
            {
                return 0;

            }
        }

        public async Task<IEnumerable<Users>> GetLocations()
        {
            await SyncAsync("user1", true);
   
            var locations = await azureSyncTable.ToListAsync();
            return locations;
        }

        public async Task<int> InsertLocation(LocationAutoCompleteResult location, string currentUser)
        {
            return await InsertLocation(new Users(location, currentUser), currentUser);
        }

        public async Task<int> InsertLocation(Users location, string currentUser)
        {
            await SyncAsync(currentUser, true);
            await azureSyncTable.InsertAsync(location);
            await SyncAsync(currentUser);
            return 1;
        }

        public async Task<IEnumerable<Users>> SelectViaUser(string currentUser)
        {
            await SyncAsync(currentUser, true);
            var Reqs = await azureSyncTable.Where(x => x.ReqFrom == currentUser).ToListAsync();
            return Reqs;
        }

        private async Task SyncAsync(string curerntUser, bool pullData = false)
        {
            try
            {
                await azureDatabase.SyncContext.PushAsync();

                if (pullData)
                {
                    //await azureSyncTable.PullAsync("allLocations", azureSyncTable.CreateQuery()); // query ID is used for incremental sync
                    await azureSyncTable.PullAsync("Users", azureSyncTable.CreateQuery());
                }
            }

            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

    }
}
