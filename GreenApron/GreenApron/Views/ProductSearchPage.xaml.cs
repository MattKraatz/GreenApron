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
        public ObservableCollection<Product> productSearchItems { get; private set; } = new ObservableCollection<Product>();

        public ProductSearchPage()
        {
            InitializeComponent();
            productSearchList.ItemsSource = productSearchItems;
        }

        public void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            productSearchList.SelectedItem = null;
        }

        public async void DoSearch(object sender, EventArgs args)
        {
            if (productSearch.Text.Length > 1)
            {
                var response = await App.SpoonManager.GetProductByQuery(productSearch.Text);
                if (response.Count > 0)
                {
                    productSearchItems.Clear();
                    foreach (Product product in response)
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
