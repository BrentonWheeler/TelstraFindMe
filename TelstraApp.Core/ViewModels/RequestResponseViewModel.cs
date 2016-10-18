using Android.App;
using MvvmCross.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelstraApp.Core.Interfaces;
using TelstraApp.Core.Models;


namespace TelstraApp.Core.ViewModels
{

    public class RequestResponseViewModel : MvxViewModel
    {
        private GeoLocation myLocation;
        private Action<GeoLocation, float> moveToLocation;
        private Action<GeoLocation> weatherPinFound;

        public RequestResponseViewModel() {

            string here = "";
            //SetupMap();
        }

        public GeoLocation MyLocation
        {
            get { return myLocation; }
            set { myLocation = value; }
        }

        private void getData(GeoLocation location)
        {
            weatherPinFound(location);
        }

        public void OnMyLocationChanged(GeoLocation location)
        {
            MyLocation = location;
            GeoLocation location2 = new GeoLocation(-27.470125, 153.0251);
            getData(location2);
        }




        public void OnMapSetup(Action<GeoLocation, float> MoveToLocation, Action<GeoLocation> WeatherPinFound)
        {
            moveToLocation = MoveToLocation;
            weatherPinFound = WeatherPinFound;

        }

    /*    public void OnMapSetup(Action<GeoLocation, float> MoveToLocation, Action<GeoLocation, Forecast> WeatherPinFound)
        {
            moveToLocation = MoveToLocation;
            weatherPinFound = WeatherPinFound;
        }
        */
    }
}
