using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace GreenApron
{
    public partial class GroceryItemDetailModal : ContentPage
    {
        public GroceryItemDetailModal(GroceryItem item)
        {
            InitializeComponent();
            this.BindingContext = item;
            slider.Maximum = item.Amount * 3;
            StepValue = 0.125;
            slider.ValueChanged += OnSliderValueChanged;
        }

        public async void OnUpdateClicked(object sender, EventArgs e)
        {
            var item = (GroceryItem)BindingContext;
            // TODO: Call WebAPI endpoint that updates a single GroceryItem by Id
            await Navigation.PopModalAsync();
        }

        public async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        public async void OnDeleteClicked(object sender, EventArgs e)
        {
            var item = (GroceryItem)BindingContext;
            var itemId = item.GroceryItemId;
            // TODO: Call WebAPI endpoint that deletes a single GroceryItem by Id
            await Navigation.PopModalAsync();
        }

        // Slider helper, from https://forums.xamarin.com/discussion/80468/how-to-add-steps-to-my-slider
        private double StepValue { get; set; }
        public void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
        {
            var newStep = Math.Round(e.NewValue / StepValue);

            slider.Value = newStep * StepValue;
        }
    }
}
