using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;

namespace GreenApron
{
    public partial class GroceryItemDetailModal : ContentPage
    {
        public GroceryItemDetailModal(GroceryItem item)
        {
            InitializeComponent();
            this.BindingContext = item;
            slider.Maximum = item.Amount * 8;
            StepValue = 0.125;
            slider.ValueChanged += OnSliderValueChanged;
            ListPlans(item.Plans);
        }

        public void ListPlans(Plan[] plans)
        {
            planList.ItemsSource = plans;
            planList.RowHeight = 45;
            planList.HeightRequest = 45 * (plans.Count() + 1);
        }

        public async void HandleTap(object sender, ItemTappedEventArgs e)
        {
            var plan = e.Item as Plan;
            var page = new PlanPage(plan, true);
            await Navigation.PushModalAsync(page);
            planList.SelectedItem = null;
        }

        public async void OnUpdateClicked(object sender, EventArgs e)
        {
            busy.IsVisible = true;
            busy.IsRunning = true;
            var item = (GroceryItem)BindingContext;
            var request = new GroceryRequest();
            request.items = new List<GroceryItem>();
            if (item.Purchased)
            {
                item.DateCompleted = DateTime.Now;
            }
            request.items.Add(item);
            var response = await App.APImanager.UpdateGroceryItems(request);
            busy.IsVisible = false;
            busy.IsRunning = false;
            if (response.success)
            {
                await Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Error", response.message, "Okay");
            }
        }

        public async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        public async void OnDeleteClicked(object sender, EventArgs e)
        {
            busy.IsVisible = true;
            busy.IsRunning = true;
            var item = (GroceryItem)BindingContext;
            item.Deleted = true;
            var request = new GroceryRequest();
            request.items = new List<GroceryItem>();
            request.items.Add(item);
            var response = await App.APImanager.UpdateGroceryItems(request);
            busy.IsVisible = false;
            busy.IsRunning = false;
            if (response.success)
            {
                await Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Error", response.message, "Okay");
            }
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
