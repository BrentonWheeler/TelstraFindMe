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
            string msg = "Hi there,\n";
            if (responseMessage.RespLunch)
                msg += "I'm an currently at lunch.\n";
            else if (responseMessage.RespMeeting)
            {
                msg += "I'm an currently in a meeting";
                if (responseMessage.RespRoom != null && responseMessage.RespRoom != "")
                {
                    msg += " in room " + responseMessage.RespRoom + "\n";
                }
                else
                {
                    msg += ".\n";
                }
            }
            else if ( responseMessage.RespHome)
            {
                msg += "I'm an currently at home today.\n";
            }

            msg += "\nYou can find me at that the location below\n";
            ResponseMsg = msg;
        }

        public void Init(Users response)
        {
            lat = (double)response.RespLocationLat;
            lng = (double)response.RespLocationLng;
            buildResponseMessage(response);

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
