using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace GreenApron
{
    public partial class HomeInventoryPage : ContentPage
    {
        public ObservableCollection<InventoryListGroup> inventoryItems { get; private set; } = new ObservableCollection<InventoryListGroup>();

        public HomeInventoryPage()
        {
            InitializeComponent();
            inventoryList.ItemsSource = inventoryItems;
            inventoryList.HeightRequest = 1000;
            GetInventoryItems();
        }

        public async void GetInventoryItems()
        {
            var response = await App.APImanager.GetInventoryItems();
            busy.IsRunning = false;
            busy.IsVisible = false;
            inventoryItems.Clear();
            if (response.success)
            {
                // Sort items by aisle for initial load
                foreach (InventoryItem item in response.InventoryItems)
                {
                    item.AmountUnit = item.Amount.ToString() + " " + item.Unit;
                    item.Count = item.Plans.Count();
                    // Find existing InventoryListGroup
                    var groupCheck = inventoryItems.SingleOrDefault(g => g.Title == item.Ingredient.aisle);
                    if (groupCheck == null)
                    {
                        inventoryItems.Add(new InventoryListGroup(item.Ingredient.aisle, item.Ingredient.aisle)
                            {
                                item
                            });
                    }
                    else
                    {
                        groupCheck.Add(item);
                    }
                }
				editBtn.IsEnabled = true;
            }
            else
            {
                await DisplayAlert("Error", response.message, "Okay");
            }
            busy.IsRunning = false;
            busy.IsVisible = false;
        }

        public void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as InventoryItem;
            var page = new PantryItemDetailModal(item);
            Navigation.PushModalAsync(page);
            inventoryList.SelectedItem = null;
        }

        public async void OpenProductSearch(object sender, EventArgs e)
        {
            var page = new ProductSearchPage("inventory");
            await Navigation.PushAsync(page);
        }

		public async void OpenRecipeSearch(object sender, EventArgs e)
		{
			var page = new RecipeSearchByProductPage(inventoryItems);
			await Navigation.PushAsync(page);
		}

        public async void EditInventory(object sender, EventArgs e)
        {
            var page = new InventoryActionPage(inventoryItems);
            await Navigation.PushAsync(page);
        }

		protected override void OnAppearing()
		{
			if (!busy.IsRunning)
			{
				busy.IsRunning = true;
				busy.IsVisible = true;
				GetInventoryItems();
			}
		}
    }
}
