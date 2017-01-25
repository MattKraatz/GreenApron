using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace GreenApron
{
    public partial class RecipeSearchPage : ContentPage
    {
        public ObservableCollection<RecipePreview> recipePageItems { get; private set; } = new ObservableCollection<RecipePreview>();
        private DateTime? _activeDate { get; set; }

        public RecipeSearchPage()
        {
            InitializeComponent();
            recipeSearchList.ItemsSource = recipePageItems;
        }

        public RecipeSearchPage(DateTime activeDate)
        {
            InitializeComponent();
            recipeSearchList.ItemsSource = recipePageItems;
            _activeDate = activeDate;
        }

        public async void DoSearch(object sender, EventArgs args)
        {
            if (recipeSearch.Text.Length > 1)
            {
				busy = Busy.Flip(busy);
                var response = await App.SpoonManager.GetRecipesByQueryAsync(recipeSearch.Text, 0);
                busy = Busy.Flip(busy);
                if (response.totalResults > 0)
                {
                    recipePageItems.Clear();
                    foreach (RecipePreview recipe in response.results)
                    {
                        recipePageItems.Add(recipe);
                    }
                }
                else
                {
                    await DisplayAlert("Error", "No products were found for this search, please try again.", "Okay");
                }
            }
            else
            {
                await DisplayAlert("Error", "Please enter a search query.", "Okay");
            }
        }

        //// Tabling this for future use in an advanced search
        //public async void GetRandomRecipe(object sender, EventArgs e)
        //{
        //    var recipes = await App.SpoonManager.GetRandomRecipeAsync();
        //    foreach (Recipe item in recipes)
        //    {
        //        recipePageItems.Add(item);
        //    }
        //}

        public void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var recipe = e.Item as RecipePreview;
            var recipePage = new RecipePage(recipe.id, _activeDate);
            Navigation.PushAsync(recipePage);
            recipeSearchList.SelectedItem = null;
        }
    }
}
