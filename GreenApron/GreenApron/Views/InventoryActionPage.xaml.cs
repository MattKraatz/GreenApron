using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace GreenApron
{
    public partial class InventoryActionPage : ContentPage
    {
        public ObservableCollection<InventoryListGroup> inventoryItems { get; private set; } = new ObservableCollection<InventoryListGroup>();

        public InventoryActionPage(ObservableCollection<InventoryListGroup> items)
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

        public async void Update(object sender, EventArgs e)
        {
            var request = new InventoryRequest();
            request.items = new List<InventoryItem>();
            foreach (InventoryListGroup group in inventoryItems)
            {
                foreach (InventoryItem item in group)
                {
                    request.items.Add(item);
                }
            }
            var response = await App.APImanager.UpdateInventoryItems(request);
            await Navigation.PopAsync();
        }

        public async void Cancel(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
