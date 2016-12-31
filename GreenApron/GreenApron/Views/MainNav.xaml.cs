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
                IconSource = "restaurant.png",
                TargetType = typeof(HomePage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "My Meal Plans",
                IconSource = "calendar.png",
                TargetType = typeof(MealPlanHomePage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Recipe Search",
                IconSource = "search.png",
                TargetType = typeof(RecipeSearchPage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "My Recipe Book",
                IconSource = "book.png",
                TargetType = typeof(RecipeCollectionPage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "My Pantry",
                IconSource = "web.png",
                TargetType = typeof(HomeInventoryPage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "My Grocery List",
                IconSource = "commerce.png",
                TargetType = typeof(GroceryListPage)
            });

            listView.ItemsSource = masterPageItems;
        }
    }
}
