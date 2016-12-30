using Plugin.Toasts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace GreenApron
{
    public partial class PlanPage : ContentPage
    {
        private Plan _activePlan { get; set; }

        public PlanPage() { }

        public PlanPage(Plan plan)
        {
            InitializeComponent();
            _activePlan = plan;
            GetRecipe(plan);
        }

        public async void GetRecipe(Plan plan)
        {
            plan.Recipe = await App.SpoonManager.GetRecipeByIdAsync(plan.RecipeId);
            CleanPage(plan);
        }

        public void CleanPage(Plan plan)
        {
            servings.IsVisible = plan.Recipe.servings > 0;
            prepMin.IsVisible = plan.Recipe.preparationMinutes > 0;
            cookMin.IsVisible = plan.Recipe.cookingMinutes > 0;
            ingredientsList.ItemsSource = plan.Recipe.extendedIngredients;
            ingredientsList.RowHeight = 20;
            ingredientsList.HeightRequest = (20 * plan.Recipe.extendedIngredients.Count()) + 1;

            this.BindingContext = plan;
        }

        public void HandleTap(object sender, EventArgs e)
        {
            ingredientsList.SelectedItem = null;
        }

        public async void OnDeleteClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Development", "You still need to implement this", "Okay");
        }

        public async void OnCookedClicked(object sender, EventArgs e)
        {
            // Call to the Database to Update the Plan and Update InventoryItems
            var response = await App.APImanager.CompletePlan(_activePlan.PlanId);
            if (response.success)
            {
                await DisplayAlert("Success", "You cooked it!", "Okay");
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
