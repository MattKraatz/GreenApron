using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace GreenApron
{
	public partial class HomePage : ContentPage
	{
		public HomePage()
		{
			InitializeComponent();
            CheckLogin();
		}

        public async void CheckLogin()
        {
            // Auto-login feature
            //if (Keys.User.Length > 0)
            //{
            //    var user = new User { Username = Keys.User, Password = Keys.Pass };
            //    AuthResponse response = await App.AuthManager.LoginAsync(user);
            //    if (!response.success)
            //    {
            //        var login = new LoginPage();
            //        await Navigation.PushModalAsync(login);
            //    } else
            //    {
            //        App.AuthManager.loggedInUser = response.user;
            //        await DisplayAlert("Hello", response.message, "Okay");
            //    }
            //} else if (App.AuthManager.loggedInUser == null)
            //{
            //    var login = new LoginPage();
            //    await Navigation.PushModalAsync(login);
            //}

            if (App.AuthManager.loggedInUser == null)
            {
                var login = new LoginPage();
                await Navigation.PushModalAsync(login);
            }
        }
    }
}
