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
                // Check local SQL database
                var user = await App.Database.GetUserAsync();
                if (user == null)
                {
                    var login = new LoginPage();
                    await Navigation.PushModalAsync(login);
                } else
                {
                    App.AuthManager.loggedInUser = user;
                }
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

        public async void Logout(object sender, EventArgs e)
        {
            var user = App.AuthManager.loggedInUser;
            await App.Database.DeleteUserAsync(user);
            App.AuthManager.loggedInUser = null;
            var login = new LoginPage();
            await Navigation.PushModalAsync(login);
        }

    }
}
