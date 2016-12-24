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
            GetInventoryItems();
        }

        public async void GetInventoryItems()
        {
            var response = await App.APImanager.GetInventoryItems();
            if (response.success)
            {
                inventoryItems.Clear();
                // Sort items by aisle for initial load
                foreach (InventoryItem item in response.InventoryItems)
                {
                    item.AmountUnit = item.Amount.ToString() + " " + item.Unit;
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
            }
            else
            {
                await DisplayAlert("Error", response.message, "Okay");
            }
        }

        public async void OpenProductSearch(object sender, EventArgs e)
        {
            var page = new ProductSearchPage("inventory");
            await Navigation.PushAsync(page);
        }

        public async void EditInventory(object sender, EventArgs e)
        {
            var page = new InventoryActionPage(inventoryItems);
            await Navigation.PushAsync(page);
        }

        public async void ToggleHandler(object sender, ToggledEventArgs e)
        {
            if (e.Value)
            {
                var switchItem = (InventorySwitch)sender;
                var item = switchItem.InventoryItem;
                var rebuy = await DisplayActionSheet("Do you want to rebuy?", "Cancel", null, new string[] { "Yes", "No"});
                if (rebuy == "Yes")
                {
                    // Add a grocery item
                    var newGI = new Ingredient() { aisle = item.Ingredient.aisle, amount = item.Amount, unit = item.Unit, image = item.Ingredient.imageURL, name = item.Ingredient.ingredientName };
                    var response2 = await App.APImanager.AddGroceryItem(newGI);
                    if (!response2.success)
                    {
                        await DisplayAlert("Error", response2.message, "Okay");
                    }
                }
                foreach (InventoryListGroup group in inventoryItems)
                {
                    foreach (InventoryItem ii in group)
                    {
                        if (ii.InventoryItemId == item.InventoryItemId)
                        {
                            // Remove the inventory item from the observable collection
                            group.Remove(ii);
                            break;
                        }
                    }
                }
                // Remove the inventory item from the WebAPI
                var response = await App.APImanager.DeleteInventoryItem(item.InventoryItemId);
                if (!response.success)
                {
                    await DisplayAlert("Error", response.message, "Okay");
                }
            }
        }
    }
}
