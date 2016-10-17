using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;
using MvvmCross.Droid.Views;
using TelstraApp.Core.ViewModels;


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
        }

    }
    //Author: Michael Kath (n9293833)
    [Activity(Label = "View for FindView")]
    public class FindView: MvxActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Find);

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

        }


    }
}
