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
        ObservableCollection<RecipePreview> _recipePageItems { get; set; } = new ObservableCollection<RecipePreview>();
        DateTime? _activeDate { get; set; }
		int _totalResults { get; set; }
		bool _endOfResults { get; set; }
		List<string> _ingreds { get; set; }

        public RecipeSearchPage()
        {
            InitializeComponent();
            recipeSearchList.ItemsSource = _recipePageItems;
			recipeSearchList.ItemAppearing += ItemAppearing;
        }

        public RecipeSearchPage(DateTime activeDate)
        {
            InitializeComponent();
            recipeSearchList.ItemsSource = _recipePageItems;
			recipeSearchList.ItemAppearing += ItemAppearing;
            _activeDate = activeDate;
        }

		public RecipeSearchPage(List<string> ingreds)
		{
			InitializeComponent();
			_ingreds = ingreds;
			recipeSearchList.ItemsSource = _recipePageItems;
			GetRecipesByIngreds();
		}

        async void DoSearch(object sender, EventArgs args)
        {
            if (recipeSearch.Text.Length > 1)
            {
				_recipePageItems.Clear();
				_totalResults = 0;
				_endOfResults = false;
				busy = Busy.Flip(busy);
                var response = await App.SpoonManager.GetRecipesByQueryAsync(recipeSearch.Text, 0);
                busy = Busy.Flip(busy);
                if (response.totalResults > 0)
				{
					_totalResults = response.totalResults;
                    foreach (RecipePreview recipe in response.results)
                    {
						recipe.image = "https://spoonacular.com/recipeImages/" + recipe.image;
                        _recipePageItems.Add(recipe);
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

		async void GetRecipesByIngreds()
		{
			var recipes = await App.SpoonManager.GetRecipeByIngreds(_ingreds,0);
			foreach (RecipeIngredsPreview recipe in recipes)
			{
				var prev = new RecipePreview();
				prev.title = recipe.title;
				prev.image = recipe.image;
				prev.id = recipe.id;
				_recipePageItems.Add(prev);
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

        void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var recipe = e.Item as RecipePreview;
            var recipePage = new RecipePage(recipe.id, _activeDate);
            Navigation.PushAsync(recipePage);
            recipeSearchList.SelectedItem = null;
        }

		// Infinite ListView Extension, borrowed from http://www.codenutz.com/lac09-xamarin-forms-infinite-scrolling-listview/
		async void ItemAppearing(object sender, ItemVisibilityEventArgs e)
		{
			var items = _recipePageItems;
			var item = e.Item;
			if (items != null && e.Item == items[items.Count - 1])
			{
				busy = Busy.Flip(busy);
				var response = await App.SpoonManager.GetRecipesByQueryAsync(recipeSearch.Text, items.Count);
				busy = Busy.Flip(busy);
				if (response.totalResults > 0)
				{
					foreach (RecipePreview recipe in response.results)
					{
						recipe.image = "https://spoonacular.com/recipeImages/" + recipe.image;
						_recipePageItems.Add(recipe);
					}
				}
				else if (!_endOfResults)
				{
					var label = new Label();
					label.Text = "No more recipes, please try another search.";
					recipeStack.Children.Add(label);
				}
			}
		}
    }
}
