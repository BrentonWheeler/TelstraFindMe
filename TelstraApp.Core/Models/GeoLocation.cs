using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelstraApp.Core.Models
{
    public class GeoLocation
    {
        public GeoLocation() { }
        public GeoLocation(double latitude, double longitude, double? altitude = null)
        {
            Latitude = latitude;
            Longitude = longitude;
            if (altitude == null)
            {
                HasAltitude = false;
            }
            else
            {
                HasAltitude = true;
            }
            Altitude = altitude;
        }

        public GeoLocation(double latitude, double longitude, string reqUser)
        {
            Latitude = latitude;
            Longitude = longitude;
            Locality = reqUser;
            HasAltitude = false;
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? Altitude { get; set; }
        public bool HasAltitude { get; set; }
        public string Locality { get; set; }
    }
}

