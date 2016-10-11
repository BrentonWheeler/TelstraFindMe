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
using System.Threading.Tasks;
using System.Threading;
using Android.Util;
using TelstraApp.Core.Database;
using TelstraApp.Droid.Views;
using MvvmCross.Core.ViewModels;
using TelstraApp.Core.ViewModels;

namespace TelstraApp.Droid.Services
{
    [Service]
    public class MyService : Service
    {
        DemoServiceBinder binder;
        public override IBinder OnBind(Intent intent)
        {
            binder = new DemoServiceBinder(this);
            return binder;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {

            new Task(() => {
                // long running code
              DoWork();
            }).Start();
            return StartCommandResult.Sticky;
        }
        public void DoWork()
        {

            var t = new Thread(async () =>
            {

                while (true)
                {
                    Log.Debug("DemoService", "Runnings");
                    // this.
                    var curerntReq = await UsersDatabase.SelectViaUser("User1");
                    Thread.Sleep(5000);

                }
            }
            );

            t.Start();
        }


        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        public class DemoServiceBinder : Binder
        {
            MyService service;

            public DemoServiceBinder(MyService service)
            {
                this.service = service;
            }

            public MyService GetDemoService()
            {
                return service;
            }
        }
    }
}