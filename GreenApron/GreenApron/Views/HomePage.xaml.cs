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
            if (Keys.User.Length > 0)
            {
                var user = new User { Username = Keys.User, Password = Keys.Pass };
                AuthResponse response = await App.AuthManager.LoginAsync(user);
                if (!response.success)
                {
                    var login = new LoginPage();
                    await Navigation.PushModalAsync(login);
                } else
                {
                    App.AuthManager.loggedInUser = response.user;
                    await DisplayAlert("Hello", response.message, "Okay");
                }
            } else if (App.AuthManager.loggedInUser == null)
            {
                var login = new LoginPage();
                await Navigation.PushModalAsync(login);
            }
        }

		public async void OpenRecipeSearch(object sender, EventArgs e)
		{
			var tab = new TabbedPage();
			tab.Title = "New Plan";

			var page = new RecipeSearchPage();
			var page2 = new RecipeCollectionPage();

			tab.Children.Add(page);
			tab.Children.Add(page2);

			await Navigation.PushAsync(tab);
		}
	}
}
