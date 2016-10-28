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

        Task<int> DeleteRequest(string employee, string currentUser);

        Task<int> InsertLocation(Users location, string currentUser);
        Task<int> InsertRequest(Employees location, string currentUser);
        Task<IEnumerable<Users>> SelectToUser( string currentUser);
        Task<IEnumerable<Users>> SelectViaUser( string currentUser, bool pushSync = false);
        Task<bool> CheckIfExists(Users location, string currentUser);
        Task<bool> CheckIfExists(Employees location, string currentUser);
        Task<int> AddResponse(AddRequest Requests, string currentUser);
        Task RunSync(string currentUser);
        Task<IEnumerable<Employees>> GetEmployees(string searchTerm, string currentUser);
        Task<int> InsertEmployee(Employees employee);
        Task SyncAsyncEmp(bool pullData = false);
        Task<String[]> GetFavourites(string currentUser);
        Task<Users> GetResponse(string currentUser, string curReq);
    }
}
