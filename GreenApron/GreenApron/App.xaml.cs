using Xamarin.Forms;

namespace GreenApron
{
    public partial class App : Application
    {
        public static AuthManager AuthManager { get; private set; }
        public static SpoonManager SpoonManager { get; private set; }
        public static APImanager APImanager { get; private set; }

        // Use DependencyService to instantiate a connection to the client database
        // https://developer.xamarin.com/guides/xamarin-forms/working-with/databases/#XamarinForms_PCL_Project
        private static AuthDatabase _AuthDatabase;
        public static AuthDatabase Database
        {
            get
            {
                if (_AuthDatabase == null)
                {
                    _AuthDatabase = new AuthDatabase(DependencyService.Get<IFileHelper>().GetLocalFilePath("TodoSQLite.db3"));
                }
                return _AuthDatabase;
            }
        }

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
