using TelstraApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelstraApp.Core.Interfaces
{
    public interface IUsersDatabase
    {
        Task<IEnumerable<Users>> GetLocations();

        Task<int> DeleteLocation(object id);

        Task<int> InsertLocation(Users location, string currentUser);
        Task<int> InsertLocation(LocationAutoCompleteResult location, string currentUser);
        Task<IEnumerable<Users>> SelectViaUser( string currentUser);
        Task<bool> CheckIfExists(Users location, string currentUser);
        Task<bool> CheckIfExists(LocationAutoCompleteResult location, string currentUser);


    }
}
