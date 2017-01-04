using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace GreenApron
{
    public partial class App : Application
    {
        public static AuthManager AuthManager { get; private set; }
        public static SpoonManager SpoonManager { get; private set; }
        public static APImanager APImanager { get; private set; }

        public App()
        {
            InitializeComponent();

            AuthManager = new AuthManager(new AuthService());
            SpoonManager = new SpoonManager(new SpoonService());
            APImanager = new APImanager(new APIservice());

            MainPage = new GreenApron.MainPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
        
    }
}
