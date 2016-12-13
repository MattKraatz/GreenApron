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
        public ObservableCollection<Recipe> recipePageItems { get; private set; } = new ObservableCollection<Recipe>();

        public RecipeSearchPage()
        {
            InitializeComponent();
            recipeSearchList.ItemsSource = recipePageItems;
        }

        public async void GetRandomRecipe(object sender, EventArgs e)
        {
            var recipes = await App.SpoonManager.GetRandomRecipeAsync();
            foreach (Recipe item in recipes)
            {
                recipePageItems.Add(item);
            }
        }

        public void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var recipe = e.Item as Recipe;
            var recipePage = new RecipePage();
            recipePage.BindingContext = recipe;
            Navigation.PushAsync(recipePage);
        }
    }
}
