using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelstraApp.Core.Models
{
    public class Location
    {
        public Location() { }
        public Location(LocationAutoCompleteResult location)
        {
            LocalizedName = location.LocalizedName;
            Rank = location.Rank;
            Key = location.Key;
        }
        public string Id { get; set; }
        public string LocalizedName { get; set; }
        public int Rank { get; set; }
        public string Key { get; set; }
    }
}
