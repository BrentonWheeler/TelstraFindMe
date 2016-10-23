
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using MvvmCross.Droid.Views;
using System;
using TelstraApp.Core.ViewModels;
using TelstraApp.Droid.Services;

namespace TelstraApp.Droid.Views
{
    [Activity(Label = "View for RequestsView")]
    public class RequestsView : MvxActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Requests);
        }



    }
    [Activity(Label = "View for ResponseView")]
    public class ResponseView : MvxActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Response);

            Intent startService1 = new Intent();
            startService1.SetClass(this, typeof(MyService));
            startService1.PutExtra("user1", true);
            StartService(startService1);
        }

    }
    //Author: Michael Kath (n9293833)
    [Activity(Label = "View for FindView")]
    public class FindView: MvxActivity
    {
      

        protected TelstraApp.Core.ViewModels.FindViewModel vm
        {
            get { return base.ViewModel as FindViewModel; }
        }
        protected override void OnCreate(Bundle bundle)
        {
            
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(bundle);
            
            SetContentView(Resource.Layout.Find);
            vm.MyEvent += Vm_MyEvent;

        }
        private void Vm_MyEvent(string msg, bool hideKeyBoard)
        {
            if (hideKeyBoard)
            {
                InputMethodManager inputManager = (InputMethodManager)GetSystemService(InputMethodService);
                var currentFocus = Window.CurrentFocus;
                inputManager.HideSoftInputFromWindow(currentFocus.WindowToken, HideSoftInputFlags.None);
            }
        
            var Toasty = Toast.MakeText(this, msg, ToastLength.Long);
            Toasty.Show();
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
        }
           
        
      }


    //Author: Michael Kath (n9293833)
    [Activity(Label = "View for FindView")]
    public class LoginView : MvxActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Login);

        }


    }


    //Author Michael Kath (n9293833)
    [Activity(Label = "FirstView")]
    public class FirstView : MvxTabActivity
    {
        protected FirstViewModel FirstViewModel
        {
            get { return base.ViewModel as FirstViewModel; }
        }
        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.FirstView);
            TabHost.TabSpec tabspec;
            //Intent intent;

            tabspec = TabHost.NewTabSpec("requests");
            tabspec.SetIndicator("Requests");
            tabspec.SetContent(this.CreateIntentFor(FirstViewModel.Requests));
            TabHost.AddTab(tabspec);

            tabspec = TabHost.NewTabSpec("find");
            tabspec.SetIndicator("Find");
            tabspec.SetContent(this.CreateIntentFor(FirstViewModel.Find));
            TabHost.AddTab(tabspec);

            //string current = FirstViewModel.Current_User();


        }




        public override void OnBackPressed()
        {
            base.OnBackPressed();
            //finish();
        }//end onBackPressed()

    }
}

