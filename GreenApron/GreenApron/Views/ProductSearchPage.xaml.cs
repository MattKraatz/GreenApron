using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GreenApron
{
    public partial class ProductSearchPage : ContentPage
    {
        public ObservableCollection<Ingredient> productSearchItems { get; private set; } = new ObservableCollection<Ingredient>();
        private string _context { get; set; }

        public ProductSearchPage(string ctx)
        {
            InitializeComponent();
            productSearchList.ItemsSource = productSearchItems;
            _context = ctx;
        }

        public void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            // Open a modal for adding the provided product and set the recipe as binding context
            var product = e.Item as Ingredient;
            var modal = new ProductDetailModal(product, _context);
            Navigation.PushModalAsync(modal);
            // Deselect the tapped item
            productSearchList.SelectedItem = null;
        }

        public async void DoSearch(object sender, EventArgs args)
        {
            if (productSearch.Text.Length > 1)
            {
                busy.IsVisible = true;
                busy.IsRunning = true;
                var response = await App.SpoonManager.GetProductByQuery(productSearch.Text);
                busy.IsVisible = false;
                busy.IsRunning = false;
                if (response.Count > 0)
                {
                    productSearchItems.Clear();
                    foreach (Ingredient product in response)
                    {
                        productSearchItems.Add(product);
                    }
                }
                else
                {
                    await DisplayAlert("Error", "No products were found for this search, please try again.", "Okay");
                }
            } else
            {
                await DisplayAlert("Error", "Please enter a search query.", "Okay");
            }
        }
    }
}
