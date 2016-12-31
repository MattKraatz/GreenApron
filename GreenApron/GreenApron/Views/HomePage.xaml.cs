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
            if (App.AuthManager.loggedInUser == null)
            {
                var login = new LoginPage();
                await Navigation.PushModalAsync(login);
            }
        }
	}
}
