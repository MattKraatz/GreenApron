using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace GreenApron
{
    public partial class MainNav : ContentPage
    {
        public ListView ListView { get { return listView; } }

        public MainNav()
        {
            InitializeComponent();

            var masterPageItems = new List<MasterPageItem>();
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Home",
                IconSource = "",
                TargetType = typeof(HomePage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Meal Planning",
                IconSource = "",
                TargetType = typeof(MealPlanHomePage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Grocery List",
                IconSource = "",
                TargetType = typeof(GroceryListPage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Recipe Search",
                IconSource = "",
                TargetType = typeof(RecipeSearchPage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Bookmarked Recipes",
                IconSource = "",
                TargetType = typeof(RecipeCollectionPage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "My Pantry",
                IconSource = "",
                TargetType = typeof(HomeInventoryPage)
            });

            listView.ItemsSource = masterPageItems;
        }
    }
}
