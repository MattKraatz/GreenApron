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

        public PlanPage(Plan plan, bool modal)
        {
            InitializeComponent();
            if (modal)
            {
                actionStack.Children.Clear();
                var button = new Button
                {
                    Text = "Cancel", TextColor = Color.White, BackgroundColor = Color.FromHex("#FF0700"),
                    HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.FillAndExpand
                };
                button.Clicked += OnCancelClicked;
                actionStack.Children.Add(button);
            }
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
            ingredientsList.RowHeight = 25;
            ingredientsList.HeightRequest = (25 * plan.Recipe.extendedIngredients.Count());
			ingredientsList.IsVisible = true;

            this.BindingContext = plan;

            busy.IsVisible = false;
            busy.IsRunning = false;

            ingredientsLabel.IsVisible = true;
            instructionsLabel.IsVisible = true;
        }

        public void HandleTap(object sender, EventArgs e)
        {
            ingredientsList.SelectedItem = null;
        }

        public async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        public async void OnDeleteClicked(object sender, EventArgs e)
        {
			var button = (Button)sender;
			button.IsEnabled = false;
			var confirm = await DisplayActionSheet("Delete this plan?", "Cancel", "Delete");
			if (confirm == "Delete")
			{
				var response = await App.APImanager.DeletePlan(_activePlan.PlanId);
				if (response.success)
				{
					await Navigation.PopAsync();
				}
				else
				{
					await DisplayAlert("Error", response.message, "Okay");
				}
			}
			button.IsEnabled = true;
        }

        public async void OnCookedClicked(object sender, EventArgs e)
        {
			var button = (Button)sender;
			button.IsEnabled = false;
            var plan = this.BindingContext as Plan;
            plan.DateCompleted = DateTime.Now;
            // Call to the Database to Update the Plan and Update InventoryItems
            var response = await App.APImanager.UpdatePlan(plan);
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
			button.IsEnabled = true;
        }
    }
}
