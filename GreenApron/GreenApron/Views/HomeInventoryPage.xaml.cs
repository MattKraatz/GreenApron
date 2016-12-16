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
        public ObservableCollection<InventoryItem> inventoryItems { get; private set; } = new ObservableCollection<InventoryItem>();

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
                foreach (InventoryItem item in response.InventoryItems)
                {
                    item.AmountUnit = item.Amount.ToString() + " " + item.Unit;
                    inventoryItems.Add(item);
                }
            }
            else
            {
                await DisplayAlert("Error", response.message, "Okay");
            }
        }
    }
}
