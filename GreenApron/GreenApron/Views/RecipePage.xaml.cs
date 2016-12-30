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
            if (this.BindingContext != null)
            {
                var page = new AddToPlanFromRecipePage(this.BindingContext as Recipe);
                await Navigation.PushAsync(page);
            }
        }

        public async void OnBookmarkClicked(object sender, EventArgs e)
        {
            if (this.BindingContext != null)
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
        }

        public async void RetrieveRecipe(int id)
        {
            var recipe = await App.SpoonManager.GetRecipeByIdAsync(id);
            CleanPage(recipe);
        }

        public void CleanPage(Recipe recipe)
        {
            this.Title = (recipe.title.Length > 0) ? recipe.title : "Recipe Detail";
            servings.IsVisible = recipe.servings > 0;
            prepMin.IsVisible = recipe.preparationMinutes > 0;
            cookMin.IsVisible = recipe.cookingMinutes > 0;
            ingredientsList.ItemsSource = recipe.extendedIngredients;
            ingredientsList.RowHeight = 20;
            ingredientsList.HeightRequest = (20 * recipe.extendedIngredients.Count()) + 1;

            this.BindingContext = recipe;
        }
    }
}
