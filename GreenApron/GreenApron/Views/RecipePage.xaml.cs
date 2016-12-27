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
        public RecipePage(int id)
        {
            InitializeComponent();
            RetrieveRecipe(id);
        }

        public async void OnAddToPlanClicked(object sender, EventArgs e)
        {
            // Add a AddToPlanFromRecipePage instance to the navigation stack
            var page = new AddToPlanFromRecipePage(this.BindingContext as Recipe);
            await Navigation.PushAsync(page);
        }

        public async void OnBookmarkClicked(object sender, EventArgs e)
        {
            var recipe = this.BindingContext as Recipe;
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

        public async void RetrieveRecipe(int id)
        {
            var recipe = await App.SpoonManager.GetRecipeByIdAsync(id);
            this.BindingContext = recipe;
        }
    }
}
