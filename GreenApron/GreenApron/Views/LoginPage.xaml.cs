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
        }

		private async void DoLogin(object sender, EventArgs e)
		{
            // grab user input from page entry elements
            var creds = (User)BindingContext;
            // attempt registration
            JsonResponse response = await App.AuthManager.LoginAsync(creds);
            if (response.success)
            {
                // if successful, set current user and route to home page
                App.AuthManager.loggedInUser = response.user;
                Navigation.InsertPageBefore(new HomePage(), this);
                await Navigation.PopAsync();
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
            if (creds.Password != creds.ConfirmPassword)
            {
                // handle mis-matched passwords
                await DisplayAlert("Error", "Your passwords don't match, please try again.", "Okay");
            }
            else
            {
                // attempt registration
                JsonResponse response = await App.AuthManager.RegisterAsync(creds);
                if (response.success)
                {
                    // if successful, set current user and route to home page
                    App.AuthManager.loggedInUser = response.user;
                    Navigation.InsertPageBefore(new HomePage(), this);
                    await Navigation.PopAsync();
                } else
                {
                    // handle unsuccessful logins
                    await DisplayAlert("Error", response.message, "Okay");
                }
            }   
		}
    }
}
