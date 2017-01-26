using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace GreenApron
{
    public partial class RecipeSearchByProductPage : ContentPage
    {
        ObservableCollection<InventoryListGroup> inventoryItems { get; set; } = new ObservableCollection<InventoryListGroup>();

        public RecipeSearchByProductPage(ObservableCollection<InventoryListGroup> items)
        {
            InitializeComponent();
            inventoryItems = items;
            inventoryList.ItemsSource = inventoryItems;
        }
        
        public void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            // Disable selection
            inventoryList.SelectedItem = null;
        }

        public async void Search(object sender, EventArgs e)
        {
			var button = (Button)sender;
			button.IsEnabled = false;
			busy = Busy.Flip(busy);

			var search = new List<string>();
			foreach (InventoryListGroup group in inventoryItems)
			{
				foreach (InventoryItem item in group)
				{
					search.Add(item.Ingredient.ingredientName);
				}
			}

			var page = new RecipeSearchPage(search);
			busy = Busy.Flip(busy);
			button.IsEnabled = true;
			await Navigation.PushAsync(page);
        }

        public async void Cancel(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
