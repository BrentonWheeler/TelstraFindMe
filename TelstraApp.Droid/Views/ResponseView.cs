using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using Android.Gms.Maps;
using MvvmCross.Droid.Views;
using Android.Gms.Maps.Model;
using TelstraApp.Core.ViewModels;
using Android.Util;
using TelstraApp.Core.Models;
using Android.Locations;

namespace TelstraApp.Droid.Views
{
    // public class RequestResponse : MvxActivity//, IOnMapReadyCallback





    [Activity(Label = "View for Response")]
    public class ResponseView : MvxActivity, ILocationListener
    {
        Criteria criteriaForLocationService;
        static readonly string TAG = "X:";
        TextView _addressText;
        Location _currentLocation;
        LocationManager _locationManager;

        string _locationProvider;
        TextView _locationText;
        //RequestResponseViewModel vm;
        protected TelstraApp.Core.ViewModels.ResponseViewModel vm
        {
            get { return base.ViewModel as ResponseViewModel; }
        }

        public void OnProviderDisabled(string provider)
        {
            throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            //throw new NotImplementedException();
        }

        //private delegate IOnMapReadyCallback OnMapReadyCallback();

        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Response);
            // mMap.MyLocationChange += Map_MyLocationChange;
            InitializeLocationManager();
        }
        void InitializeLocationManager()
        {
            _locationManager = (LocationManager)GetSystemService(LocationService);
            criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine
            };
            IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

            if (acceptableLocationProviders.Any())
            {
                _locationProvider = acceptableLocationProviders.First();
            }
            else
            {
                _locationProvider = string.Empty;
            }
            Log.Debug(TAG, "Using " + _locationProvider + ".");
        }
        protected override void OnResume()
        {
            base.OnResume();
            _locationManager.RequestLocationUpdates(_locationProvider, 1000, 0, this);
            _locationManager.RequestSingleUpdate(criteriaForLocationService, this, null);
        }
        protected override void OnPause()
        {
            base.OnPause();
            _locationManager.RemoveUpdates(this);
        }




        void DisplayAddress(Address address)
        {
            if (address != null)
            {
                StringBuilder deviceAddress = new StringBuilder();
                for (int i = 0; i < address.MaxAddressLineIndex; i++)
                {
                    deviceAddress.AppendLine(address.GetAddressLine(i));
                }
                // Remove the last comma from the end of the address.
                _addressText.Text = deviceAddress.ToString();
            }
            else
            {
                _addressText.Text = "Unable to determine the address. Try again in a few minutes.";
            }
        }
        public async void OnLocationChanged(Location location)
        {
            _currentLocation = location;
            if (_currentLocation == null)
            {
                _locationText.Text = "Unable to determine your location. Try again in a short while.";
            }
            else
            {
                vm.myLat = (float)_currentLocation.Latitude;
                vm.myLong = (float)_currentLocation.Longitude;
            }
        }
        /* public override void OnBackPressed()
{
    base.OnBackPressed();
    SetContentView(Resource.Layout.Re);
}
*/
    }
}