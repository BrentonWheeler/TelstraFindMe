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
        private double lat = 0.0;
        private double lng = 0.0;
        private string responseMsg;

        public string ResponseMsg
        {
            get
            {
                return responseMsg;
            }

            set
            {
                SetProperty(ref responseMsg, value);
            }
        }



        public RequestResponseViewModel() {

            
        }

        private void buildResponseMessage(Users responseMessage)
        {

        }

        public void Init(Users response)
        {
            lat = (double)response.RespLocationLat;
            lng = (double)response.RespLocationLng;

            BuildLocationMessage message = new BuildLocationMessage(response);

            ResponseMsg = message.message;

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
            GeoLocation location2 = new GeoLocation(lat, lng);
            getData(location2);
        }




        public void OnMapSetup(Action<GeoLocation, float> MoveToLocation, Action<GeoLocation> WeatherPinFound)
        {
            moveToLocation = MoveToLocation;
            weatherPinFound = WeatherPinFound;

        }
    }
}
