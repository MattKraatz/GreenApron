using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace GreenApron
{
    public partial class GroceryListPage : ContentPage
    {
        public ObservableCollection<GroceryItem> groceryItems { get; private set; } = new ObservableCollection<GroceryItem>();

        public GroceryListPage()
        {
            InitializeComponent();
            groceryList.ItemsSource = groceryItems;
            GetGroceryItems();
        }

        public async void GetGroceryItems()
        {
            var response = await App.APImanager.GetGroceryItems();
            if (response.success)
            {
                foreach (GroceryItem item in response.GroceryItems)
                {
                    item.AmountUnit = item.Amount.ToString() + " " + item.Unit;
                    groceryItems.Add(item);
                }
            } else
            {
                await DisplayAlert("Error", response.message, "Okay");
            }
        }

        public async void MarkAsBought(object sender, EventArgs e)
        {
            var request = new GroceryRequest();
            request.items = new List<GroceryItem>();
            foreach (GroceryItem item in groceryItems)
            {
                request.items.Add(item);
            }
            var response = await App.APImanager.UpdateGroceryItems(request);
            var test = response;
            if (response.success)
            {
                groceryItems.Clear();
            } else
            {
                await DisplayAlert("Error", response.message, "Okay");
            }
        }
    }
}
