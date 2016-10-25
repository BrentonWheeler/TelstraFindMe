
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

        public async Task<int> DeleteRequest(string employee, string currentUser)
        {
            try {

                await SyncAsync(currentUser, true);
                var location = await azureSyncTable.Where(x => x.ReqTo == employee && currentUser == x.ReqFrom).ToListAsync();
                if (location.Any())
                {
                    await azureSyncTable.DeleteAsync(location.FirstOrDefault());
                    await SyncAsync(currentUser);
                    return 0;
                }
                else
                {
                    return 1;

                }

            }
            catch
            {
                return 1;
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
            Users findReq = new Users(location, currentUser);
            return await InsertLocation(findReq, currentUser);

           
        }

        public async Task<int> InsertLocation(Users reqLocation, string currentUser)
        {
            try
            {
                await SyncAsync(currentUser, true);
                await azureSyncTable.InsertAsync(reqLocation);
                //await InsertFavourite(currentUser, reqLocation.ReqTo);
                await SyncAsync(currentUser);
                return 0;
            }
            catch
            {
                return 1;
            }

            
        }

        public async Task<Users> GetResponse(string currentUser, string curReq)
        {
            var currentReqs = await azureSyncTable.Where(x => x.ReqTo == curReq && x.ReqFrom == currentUser).ToListAsync();

            return currentReqs.FirstOrDefault();

        }

        public async Task<int> AddResponse(AddRequest Requests, string currentUser)
        {

           var currentReqs = await azureSyncTable.Where(x => x.ReqTo == Requests.UserNameReq && x.ReqFrom == currentUser).ToListAsync();


            //return currentReqs = await azureSyncTable.Where(x =><YOUR_LIST>.Any(y=>y.ReqTo == currentUser && y.reqFrom == x.RequestersName)));
            //var arrayReq = new[] { "User5", "user3" };

            //var currentReqs1 = await azureSyncTable<Users>.Where(x => arrayReq.Contains(x.ReqFrom) && x.ReqTo == currentUser));

            //var currentReqs1 = await azureSyncTable.Where(x => x.ReqTo == currentUser).ToListAsync();

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

        private async Task<int> InsertFavourite(string currentUser, string searchEmployee)
        {
            var currentEmployees = await employeeSyncTable.Where(x => x.UserName == currentUser).ToListAsync();
            try
            {
                Employees currentEmployee = currentEmployees.FirstOrDefault();
                currentEmployee.UpdateFavourites(searchEmployee);
                await SyncAsync(currentUser, true);
                await employeeSyncTable.UpdateAsync(currentEmployee);
                await SyncAsync(currentUser);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }

            return 1;
        }

        public async Task<String[]> GetFavourites(string currentUser)
        {
            var emp1 = await employeeSyncTable.Where(x => x.UserName == currentUser).ToListAsync();
            Employees employee = emp1.FirstOrDefault();
            return employee.RetrieveFavourites();
        }

        public async Task<IEnumerable<Employees>> GetEmployees(string searchterm, string currentUser)
        {
            List<Employees> Employee = null;
            try
            {
                Employee = await employeeSyncTable.Where(x => x.UserName != currentUser && x.UserName.Contains(searchterm)).OrderBy(x => x.UserName).ToListAsync();
                
                List<Employees> emp1 = new List<Employees>(Employee);
                var searchupper = searchterm.ToUpper();
                foreach (var emp in emp1)
                {
                    var emp_user = emp.UserName.ToUpper();

                    if (emp_user == searchupper)
                    {
                        Employee.Remove(emp);
                        Employee.Insert(0,emp);
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
                return Employee;
        }

        public async Task<IEnumerable<Users>> SelectViaUser(string currentUser, bool pushSync = false)
        {   
            if (pushSync)
            {
                await SyncAsync(currentUser, pushSync);
            }
          
            // await SyncAsyncEmp(true);
            var result = await azureSyncTable.Where(x => x.ReqFrom == currentUser).OrderByDescending(x => x.ReqTime).ToListAsync();

            return result;
        }

        public async Task<IEnumerable<Users>> SelectToUser(string currentUser)
        {
            await SyncAsync(currentUser, true);
            // await SyncAsyncEmp(true);
            //var Reqs = await azureSyncTable.Where(x => x.ReqFrom == currentUser).OrderByDescending(x => x.ReqTime).ToListAsync();
            //var Reqs = await azureSyncTable.Where(x => x.ReqTo == currentUser && x.HasResponded == true).OrderByDescending(x => x.ReqTime).ToListAsync();
            var Reqs = await azureSyncTable.ToListAsync();
            return Reqs;
        }

        public async Task RunSync(string currentUser)
        {
            await SyncAsync(currentUser, true);
        }

        public async Task SyncAsyncEmp(bool pullData = false)
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
