using Android.Content;
using MvvmCross.Droid.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;
using TelstraApp.Core.Interfaces;
using MvvmCross.Platform;
using TelstraApp.Droid.Database;
using TelstraApp.Droid.Services;
using TelstraApp.Core.Database;

namespace TelstraApp.Droid
{
    public class Setup : MvxAndroidSetup
    {
        public Setup(Context applicationContext) : base(applicationContext)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new TelstraApp.Core.App();
        }

        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }


        protected override void InitializeFirstChance()
        {
            Mvx.LazyConstructAndRegisterSingleton<ISqlite, SqliteDroid>();
            Mvx.LazyConstructAndRegisterSingleton<IDialogService, DialogService>();
            Mvx.LazyConstructAndRegisterSingleton<IUsersDatabase, UsersDatabase>();
            // Mvx.LazyConstructAndRegisterSingleton<IAzureDatabase, AzureDatabase>();
            base.InitializeFirstChance();
        }
    }
}
