
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using MvvmCross.Droid.Views;
using System;
using System.Threading;
using System.Threading.Tasks;
using TelstraApp.Core.ViewModels;
using TelstraApp.Droid.Services;

namespace TelstraApp.Droid.Views
{
    [Activity(Label = "View for RequestsView")]
    public class RequestsView : MvxActivity
    {
        public RequestsViewModel vm
        {
            get { return base.ViewModel as RequestsViewModel; }
        }

        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Requests);
        }

        protected override async void OnResume()
        {
            base.OnResume();
            await vm.RetrieveRequests();
        }



    }


//Author: Michael Kath (n9293833)
    [Activity(Label = "View for FindView")]
    public class FindView: MvxActivity
    {


        public FindViewModel vm
        {
            get { return base.ViewModel as FindViewModel; }
        }
        private bool RunProcess = true;

        protected override void OnCreate(Bundle bundle)
        {
            
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(bundle);
          
            SetContentView(Resource.Layout.Find);
            vm.ToastNotifcation += SendToastNotification;
            RunProcess = true;
            SyncWithDB();
        }


        protected override void OnResume()
        {
            base.OnResume();
            RunProcess = true;
            //SyncWithDB();

        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            RunProcess = false;
        }

        protected override void OnPause()
        {
            base.OnPause();
            RunProcess = false;

        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            vm.clearSearch();
        }

        public async void SyncWithDB()
        {
            bool completed = true;

            SendToastNotification("Syncing Contacts", false);
            await vm.populateList();
            if (await vm.RetrieveItemsFromDB())
            {
                //await vm.populateList();
            }
            SendToastNotification("Syncing Completed", false);

     /*       await Task.Run(async () =>
             {
                 while (RunProcess)
                 {
                     if (completed)
                     {
                         completed = false;
                         if (await vm.RetrieveItemsFromDB())
                         {
                             completed = await vm.populateList();
                         }
                         else
                         {
                             completed = true;
                         }
                         Thread.Sleep(10000);
                     }
                 }

             }); */
        }

        private void SendToastNotification(string msg, bool hideKeyBoard)
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
    [Activity(Label = "View for AdminView")]
    public class AdminView : MvxActivity
    {
     protected override void OnCreate(Bundle bundle)
     {
         RequestWindowFeature(WindowFeatures.NoTitle);
         base.OnCreate(bundle);

         SetContentView(Resource.Layout.Admin);
     }
    }

    [Activity(Label = "View for RequestResponse1View")]
    public class RequestResponse1View: MvxActivity
    {
     protected override void OnCreate(Bundle bundle)
     {
         RequestWindowFeature(WindowFeatures.NoTitle);
         base.OnCreate(bundle);

         SetContentView(Resource.Layout.RequestResponseNoMap);
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
            TabHost.TabSpec tabspec1;
            //Intent intent;
            Color color = new Color(0, 177, 235);

            tabspec = TabHost.NewTabSpec("requests");
            tabspec.SetIndicator("", Resources.GetDrawable(Resource.Drawable.request7_icon));
            tabspec.SetContent(this.CreateIntentFor(FirstViewModel.Requests));
            TabHost.AddTab(tabspec);
            
            TabWidget.GetChildAt(0).SetBackgroundColor(color);

            tabspec1 = TabHost.NewTabSpec("find");
            tabspec1.SetIndicator("", Resources.GetDrawable(Resource.Drawable.find1_icon));

            tabspec1.SetContent(this.CreateIntentFor(FirstViewModel.Find));
            TabHost.AddTab(tabspec1);

            TabWidget.GetChildAt(1).SetBackgroundColor(color);


        }


        public override void OnBackPressed()
         {
             base.OnBackPressed();
         }//end onBackPressed()

        }
    }

