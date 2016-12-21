using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace GreenApron
{
    public partial class ProductDetailModal : ContentPage
    {
        public ProductDetailModal(Product item)
        {
            InitializeComponent();
            this.BindingContext = item;
            foreach (string unit in UnitOptions)
            {
                unitPicker.Items.Add(unit);
            }
        }

        public string[] UnitOptions = new string[] { "Tablespoons", "Teaspoons", "Cups", "Pints", "Quarts", "Gallons", "Pounds", "Ounces" };

        public async void OnAddClicked(object sender, EventArgs e)
        {
            var item = (Product)BindingContext;
            var qty = Convert.ToDouble(qtyEntry.Text);
            var unit = UnitOptions[unitPicker.SelectedIndex];
            // TODO: Call WebAPI endpoint that adds a GroceryItem
            await Navigation.PopModalAsync();
        }

        public async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}
