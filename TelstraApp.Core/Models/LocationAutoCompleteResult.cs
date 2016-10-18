using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net.Attributes;

namespace TelstraApp.Core.Models
{
    public class LocationAutoCompleteResult
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int Version { get; set; }
        public string Key { get; set; }
        public string Type { get; set; }
        public int Rank { get; set; }
        public string LocalizedName { get; set; }
        [JsonProperty("Country.ID")]
        public string CountryId { get; set; }
        [JsonProperty("Country.LocalizedName")]
        public string CountryLocalizedName { get; set; }
        [JsonProperty("AdministrativeArea.ID")]
        public string AdministrativeAreaId { get; set; }
        [JsonProperty("AdministrativeArea.LocalizedName")]
        public string AdministrativeAreaLocalizedName { get; set; }
  
    }
}
