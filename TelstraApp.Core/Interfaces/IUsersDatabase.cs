using TelstraApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelstraApp.Core.ViewModels;

namespace TelstraApp.Core.Interfaces
{
    public interface IUserDatabase
    {
        Task<IEnumerable<Users>> GetLocations();

        Task<int> DeleteRequest(object id, string currentUser);

        Task<int> InsertLocation(Users location, string currentUser);
        Task<int> InsertLocation(Employees location, string currentUser);
        Task<IEnumerable<Users>> SelectToUser( string currentUser);
        Task<IEnumerable<Users>> SelectViaUser( string currentUser, bool pushSync = false);
        Task<bool> CheckIfExists(Users location, string currentUser);
        Task<bool> CheckIfExists(Employees location, string currentUser);
       
        Task<int> AddResponse(Users user, string currentUser);
        Task RunSync(string currentUser);
        Task<IEnumerable<Employees>> GetEmployees(string searchTerm, string currentUser);
        Task<int> InsertEmployee(Employees employee);
        Task SyncAsyncEmp(bool pullData = false);
        Task<String[]> GetFavourites(string currentUser);
        Task<Users> GetResponse(string currentUser, string curReq);
        Task<List<Users>> GetReqUser(string currentUser, List<ReceivedRequest> selectedRequests);
        Task<List<Users>> GetReqUserFromNameArray(string currentUser, string[] userNameList);
    }
}
