using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GreenApron
{
    public partial class LoginPage : TabbedPage
    {
        public LoginPage()
        {
            InitializeComponent();
            // set viewmodel for this page
            BindingContext = new User();
            AttachFocusEvents();
        }

		private async void DoLogin(object sender, EventArgs e)
		{
            busy.IsVisible = true;
            busy.IsRunning = true;
            // grab user input from page entry elements
            var creds = (User)BindingContext;
            // attempt registration
            AuthResponse response = await App.AuthManager.LoginAsync(creds);
            if (response.success)
            {
                // if successful, set current user and route to home page
                await App.Database.AddUserAsync(response.user);
                App.AuthManager.loggedInUser = response.user;
                await Navigation.PopModalAsync();
            }
            else
            {
                // handle unsuccessful logins
                await DisplayAlert("Error", response.message, "Okay");
            }
        }

		private async void DoRegistration(object sender, EventArgs e)
		{
            // grab user input from page entry elements
			var creds = (User)BindingContext;
            creds.FirstName = "demo";
            creds.LastName = "day";
            if (creds.Password != creds.ConfirmPassword)
            {
                // handle mis-matched passwords
                await DisplayAlert("Error", "Your passwords don't match, please try again.", "Okay");
            }
            else
            {
                // attempt registration
                AuthResponse response = await App.AuthManager.RegisterAsync(creds);
                if (response.success)
                {
                    // if successful, set current user and route to home page
                    await App.Database.AddUserAsync(response.user);
                    App.AuthManager.loggedInUser = response.user;
                    Navigation.InsertPageBefore(new HomePage(), this);
                    await Navigation.PopModalAsync();
                } else
                {
                    // handle unsuccessful logins
                    await DisplayAlert("Error", response.message, "Okay");
                }
            }   
		}

        private void AttachFocusEvents()
        {
            userEntry.Completed += (s, e) => passEntry.Focus();
            userReg.Completed += (s, e) => passReg.Focus();
            passReg.Completed += (s, e) => confReg.Focus();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
