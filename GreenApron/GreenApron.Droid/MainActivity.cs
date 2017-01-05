using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;

namespace GreenApron.Droid
{
    [Activity(Label = "GreenApron", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            global::Xamarin.Forms.Forms.Init(this, bundle);

            LoadApplication(new App());
        }

        public override void OnBackPressed()
        {
            if (this.DoBack)
            {
                base.OnBackPressed();
            }
        }

        // Disable the back button on android
        // http://stackoverflow.com/questions/28767357/prevent-closing-by-back-button-in-xamarin-forms-on-android
        public bool DoBack
        {
            get
            {
                MasterDetailPage mainPage = App.Current.MainPage as MasterDetailPage;

                if (mainPage != null)
                {
					if (mainPage.Navigation.ModalStack.Count > 0)
					{
						return true;
					}

                    bool canDoBack = mainPage.Detail.Navigation.NavigationStack.Count > 1 || mainPage.IsPresented;

                    // we are on a top level page and the Master menu is NOT showing
                    if (!canDoBack)
                    {
                        // don't exit the app just show the Master menu page
                        mainPage.IsPresented = true;
                        return false;
                    }
                    else
					{
                        return true;
                    }
                }
                return true;
            }


        }

		// Handle any exceptions
		void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
		}

	}
}

