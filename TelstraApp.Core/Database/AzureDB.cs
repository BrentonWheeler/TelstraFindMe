
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
using TelstraApp.Core.ViewModels;

namespace TelstraApp.Core.Database
{
    public class AzureDB : IUserDatabase
    {
        private MobileServiceClient azureDatabase;
        private IMobileServiceSyncTable<Users> azureSyncTable;
        private IMobileServiceSyncTable<Employees> employeeSyncTable;
        public AzureDB()
        {
            azureDatabase = Mvx.Resolve<IAzureDB>().GetMobileServiceClient();
            azureSyncTable = azureDatabase.GetSyncTable<Users>();
            employeeSyncTable = azureDatabase.GetSyncTable<Employees>();
        }

        public async Task<bool> CheckIfExists(Employees location, string currentUser)
        {
            var exists = await CheckIfExists(new Users(location, currentUser), currentUser);
            return exists;
        }

        public async Task<bool> CheckIfExists(Users UserReq, string currentUser)
        {
            await SyncAsync(currentUser, true);
            var requests = await azureSyncTable.Where(x => x.ReqTo == UserReq.ReqTo && x.ReqFrom == currentUser).ToListAsync();
            return requests.Any();
        }

        public async Task<int> DeleteRequest(object id, string currentUser)
        {
            await SyncAsync(currentUser, true );
            var location = await azureSyncTable.Where(x => x.ReqTo == id.ToString()).ToListAsync();
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
            await SyncAsync("user111111", true);
   
            var locations = await azureSyncTable.ToListAsync();
            return locations;
        }

        public async Task<int> InsertLocation(Employees location, string currentUser)
        {
            return await InsertLocation(new Users(location, currentUser), currentUser);
        }

        public async Task<int> InsertLocation(Users reqLocation, string currentUser)
        {
            await SyncAsync(currentUser, true);
            await azureSyncTable.InsertAsync(reqLocation);
            await SyncAsync(currentUser);
            return 1;
        }



        public async Task<int> AddResponse(AddRequest Requests, string currentUser)
        {
            var currentReqs = await azureSyncTable.Where(x => x.ReqTo == Requests.UserNameReq && x.ReqFrom == currentUser).ToListAsync();

            if (currentReqs.Any())
            {
                Users currentReq = currentReqs.FirstOrDefault();
                currentReq.CreateResponse(true, "Room 44");
                await SyncAsync(currentUser, true);
                await azureSyncTable.UpdateAsync(currentReq);
                await SyncAsync(currentUser);
                return 1;
            }
            else
            {
                return 0;
            }
        }
        public async Task<int> InsertEmployee(Employees employee)
        {
            
            try
            {
                await SyncAsyncEmp(true);
                await employeeSyncTable.InsertAsync(employee);
                await SyncAsyncEmp();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return 1;
        }

        public async Task<IEnumerable<Employees>> GetEmployees(string searchterm, string currentUser)
        {
            //await InsertEmployee(new Users(loc, currentUser));
            //await SyncAsyncEmp(true);
           // await InsertEmployee(emp);
            //await SyncAsyncEmp();
            IEnumerable<Employees> emp1 = null;
            try
            {
                //await SyncAsyncEmp(true);
                emp1 = await employeeSyncTable.Where(x => x.UserName != currentUser && x.UserName.Contains(searchterm)).ToListAsync();

                //emp1 = await employeeSyncTable.Where(x => x.UserName != currentUser).ToListAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
                return emp1;
        }

        public async Task<IEnumerable<Users>> SelectViaUser(string currentUser)
        {
            await SyncAsync(currentUser, true);
            // await SyncAsyncEmp(true);
            //var Reqs = await azureSyncTable.Where(x => x.ReqFrom == currentUser).OrderByDescending(x => x.ReqTime).ToListAsync();
            var Reqs = await azureSyncTable.Where(x => x.ReqFrom == currentUser && x.HasResponded == true).OrderByDescending(x => x.ReqTime).ToListAsync();
            var Reqs1 = await azureSyncTable.Where(x => x.ReqFrom == currentUser && x.HasResponded == false).OrderByDescending(x => x.ReqTime).ToListAsync();
            var result = Reqs.Union(Reqs1);
            return result;
        }

        public async Task RunSync(string currentUser)
        {
            await SyncAsync(currentUser, true);
        }

        private async Task SyncAsyncEmp(bool pullData = false)
        {
            try
            {
                await azureDatabase.SyncContext.PushAsync();

                if (pullData)
                {
                    await employeeSyncTable.PullAsync("Employee", employeeSyncTable.CreateQuery());
                }
            }

            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }
        private async Task SyncAsync(string curerntUser, bool pullData = false)
        {
            try
            {
                await azureDatabase.SyncContext.PushAsync();

                if (pullData)
                {
                    await azureSyncTable.PullAsync("Users", azureSyncTable.CreateQuery().Where(x=>x.ReqFrom == curerntUser));
                }
            }

            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

    }
}
