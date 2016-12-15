using Plugin.Toasts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace GreenApron
{
    public partial class RecipePage : ContentPage
    {
        public RecipePage()
        {
            InitializeComponent();
        }

        public async void OnAddToPlanClicked(object sender, EventArgs e)
        {
            // Add a AddToPlanFromRecipePage instance to the navigation stack
            var page = new AddToPlanFromRecipePage();
            await Navigation.PushAsync(page);
        }

        public async void OnBookmarkClicked(object sender, EventArgs e)
        {
            var recipe = App.SpoonManager.selectedRecipe;
            var newBookmark = new BookmarkRequest { userId = App.AuthManager.loggedInUser.UserId, RecipeId = recipe.id, Title = recipe.title, ImageURL = recipe.image };
            var response = await App.APImanager.AddBookmark(newBookmark);
            if (response.success)
            {
                await DisplayAlert("Success", "Bookmark Added Successfully", "Okay");
            }
            else
            {
                // handle failure
                await DisplayAlert("Error", response.message, "Okay");
            }
        }
    }
}
