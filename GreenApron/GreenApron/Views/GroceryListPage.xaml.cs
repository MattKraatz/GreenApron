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
        public ObservableCollection<GroceryListGroup> groceryItems { get; private set; } = new ObservableCollection<GroceryListGroup>();

        public GroceryListPage()
        {
            InitializeComponent();
            groceryList.ItemsSource = groceryItems;
            groceryList.HeightRequest = 1000;
            GetGroceryItems();
        }

        public async void GetGroceryItems()
        {
            var response = await App.APImanager.GetGroceryItems();
            busy.IsRunning = false;
            busy.IsVisible = false;
            if (response.success)
            {
                groceryItems.Clear();
                foreach (GroceryItem item in response.GroceryItems)
                {
                    item.Count = item.Plans.Count();
                    // Find existing GroceryListGroup
                    var groupCheck = groceryItems.SingleOrDefault(g => g.Title == item.Ingredient.aisle);
                    if (groupCheck == null)
                    {
                        groceryItems.Add(new GroceryListGroup(item.Ingredient.aisle, item.Ingredient.aisle)
                            {
                                item
                            });
                    }
                    else
                    {
                        groupCheck.Add(item);
                    }
                }
            } else
            {
                await DisplayAlert("Error", response.message, "Okay");
            }
        }
        
        public async void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            var ingredient = e.Item as GroceryItem;
            var modal = new GroceryItemDetailModal(ingredient);
            await Navigation.PushModalAsync(modal);
            // Unselect the item after pushing the modal
            groceryList.SelectedItem = null;
        }

        public async void MarkAsBought(object sender, EventArgs e)
        {
            busy.IsRunning = true;
            busy.IsVisible = true;
            var request = new GroceryRequest();
            request.items = new List<GroceryItem>();
            foreach (GroceryListGroup group in groceryItems)
            {
                foreach (GroceryItem item in group)
                {
					
                    item.Purchased = true;
                    request.items.Add(item);
                }
            }
            var response = await App.APImanager.UpdateGroceryItems(request);
            busy.IsRunning = false;
            busy.IsVisible = false;
            if (response.success)
            {
                groceryItems.Clear();
            } else
            {
                await DisplayAlert("Error", response.message, "Okay");
            }
        }

        public async void OpenActionView(object sender, EventArgs e)
        {
            var page = new GroceryListActionPage(groceryItems);
            await Navigation.PushAsync(page);
        }

        public async void OpenProductSearch(object sender, EventArgs e)
        {
            var page = new ProductSearchPage("grocery");
            await Navigation.PushAsync(page);
        }
    }
}
