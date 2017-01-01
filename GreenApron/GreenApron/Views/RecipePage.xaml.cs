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
        private DateTime? _activeDate { get; set; }

        public RecipePage(int id)
        {
            InitializeComponent();
            RetrieveRecipe(id);
            CheckBookmark(id);
        }

        public RecipePage(int id, DateTime? activeDate)
        {
            InitializeComponent();
            _activeDate = activeDate;
            RetrieveRecipe(id);
            CheckBookmark(id);
        }

        public async void OnAddToPlanClicked(object sender, EventArgs e)
        {
            if (this.BindingContext != null)
            {
                var recipe = this.BindingContext as Recipe;
                // If this page was instantiated from meal planning home, it would already have a date attached
                // If no date is attached, show the calendar view
                if (_activeDate == null)
                {
                    var page = new AddToPlanFromRecipePage(recipe);
                    await Navigation.PushAsync(page);
                // If a date is attached, just jump straight to posting the new plan
                } else
                {
                    var mealStrings = new[] { "Breakfast", "Lunch", "Dinner", "Snack", "Dessert" };
                    var action = await DisplayActionSheet("Which Meal?", "Cancel", null, mealStrings);
                    if (mealStrings.Contains(action))
                    {
                        // Post this Meal Plan to WebAPI
                        var newPlan = new PlanRequest { userId = App.AuthManager.loggedInUser.UserId, date = _activeDate, meal = action, recipe = recipe };
                        JsonResponse response = await App.APImanager.AddPlan(newPlan);
                        // On Success, pop this page from Navigation
                        if (response.success)
                        {
                            var navStack = this.Navigation.NavigationStack;
                            // Remove the search page before popping, jumps back to Meal Plan home page
                            Navigation.RemovePage(navStack[navStack.Count - 2]);
                            await Navigation.PopAsync();
                        }
                        else
                        {
                            // handle failure
                            await DisplayAlert("Error", response.message, "Okay");
                        }
                    }
                }
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

        public void CheckBookmark(int id)
        {
            // TODO: Write a WebAPI Endpoint that accepts a recipe Id and user Id and returns true or false
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

        public void HandleTap(object sender, EventArgs e)
        {
            ingredientsList.SelectedItem = null;
        }
    }
}
