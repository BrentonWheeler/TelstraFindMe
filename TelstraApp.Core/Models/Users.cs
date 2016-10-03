using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelstraApp.Core.Models
{
    public class Users
    {
        public Users() { }
        public Users(Employees location, string currentUser)
        {
            LocalizedName = location.userName;
            //Rank = location.Rank;
           // Key = location.Key;
            ReqFrom = currentUser;
            ReqTo = location.userName;
            ReqTime = DateTime.Now;
            ReqDate = DateTime.Now.ToString("DD/MM/YYYY");
            HasResponded = false;
        }

        public void CreateResponse(bool meeting, string currentlyAt)
        {
            this.RespMeeting = meeting;
            this.RespCurrentlyAt = currentlyAt;
            this.HasResponded = true;
        }
        public string Id { get; set; }
        public string LocalizedName { get; set; }
        public int Rank { get; set; }
        public string Key { get; set; }
        public string ReqTo { get; set; }
        public string ReqFrom { get; set; }
        public DateTime ReqTime { get; set; }

        public bool HasResponded { get; set; }

        public string ReqDate { get; set; }
        public string RespCurrentlyAt { get; set; }
        public float RespLocationLat { get; set; }
        public float RespLocationLng { get; set; }
        public bool RespMeeting { get; set; }
        public bool RespLunch { get; set; }
        public bool RespHome{ get; set; }
        public string RespRoom { get; set; }
        //public List<string> Favourite { get; set; }
    }
}
