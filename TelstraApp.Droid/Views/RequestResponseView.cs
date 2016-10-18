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

namespace TelstraApp.Droid.Views
{
    // public class RequestResponse : MvxActivity//, IOnMapReadyCallback





    [Activity(Label = "View for RequestResponse")]
    public class RequestResponseView : MvxActivity, IOnMapReadyCallback
    { 
        private GoogleMap mMap;

        //RequestResponseViewModel vm;
        protected TelstraApp.Core.ViewModels.RequestResponseViewModel vm
        {
            get { return base.ViewModel as RequestResponseViewModel; }
        }
        //private delegate IOnMapReadyCallback OnMapReadyCallback();

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.RequestResponse);
           
           // mMap.MyLocationChange += Map_MyLocationChange;
            SetupMap();
        }
        public void OnMapReady(GoogleMap googleMap)
        {

            vm.OnMapSetup(MoveToLocation, AddWeatherPin);
            mMap = googleMap;
            mMap.MyLocationEnabled = true;
            mMap.MyLocationChange += Map_MyLocationChange;
        }

        private void Map_MyLocationChange(object sender, GoogleMap.MyLocationChangeEventArgs e)
        {
            mMap.MyLocationChange -= Map_MyLocationChange;
            var location = new GeoLocation(e.Location.Latitude, e.Location.Longitude, e.Location.Altitude);
            //MoveToLocation(location);
            vm.OnMyLocationChanged(location);
        }

        private void MoveToLocation(GeoLocation geoLocation, float zoom = 18)
        {
            CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
            builder.Target(new LatLng(geoLocation.Latitude, geoLocation.Longitude));
            builder.Zoom(zoom);
            var cameraPosition = builder.Build();
            var cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

           

            mMap.MoveCamera(cameraUpdate);
        }


        private void AddWeatherPin(GeoLocation location)
        {
            var markerOptions = new MarkerOptions();
            markerOptions.SetPosition(new LatLng(location.Latitude, location.Longitude));
            markerOptions.SetTitle(location.Locality);
            mMap.AddMarker(markerOptions);
            MoveToLocation(location);

        }

        public void OnMapReadyOLD(GoogleMap googleMap)
        {
            mMap = googleMap;



            LatLng latlng = new LatLng(40.776408, -73.97055);
            CameraUpdate camera = CameraUpdateFactory.NewLatLngZoom(latlng, 10);
            mMap.MoveCamera(camera);
            
            MarkerOptions options = new MarkerOptions()
            .SetPosition(latlng)
            .SetTitle("New York")
            .SetSnippet("AKA: The Big Apple")
            .Draggable(true);

            

            mMap.AddMarker(options);

            mMap.MarkerDragEnd += mMap_MarkerDragEnd;
        }

        private void mMap_MarkerDragEnd(object sender, GoogleMap.MarkerDragEndEventArgs e)
        {
            LatLng pos = e.Marker.Position;
           
        }



       private void SetupMap()
        {
            if (mMap == null)
            {
                FragmentManager.FindFragmentById<MapFragment>(Resource.Id.weathermap).GetMapAsync(this);

            }
        } 
    }
}