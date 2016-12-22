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
    }
}
