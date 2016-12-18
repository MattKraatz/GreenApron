using Plugin.Toasts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace GreenApron
{
    public partial class PlanPage : ContentPage
    {
        private Plan _activePlan { get; set; }

        public PlanPage() { }

        public PlanPage(Plan plan)
        {
            InitializeComponent();
            this.BindingContext = plan;
            _activePlan = plan;
        }

        public async void OnCookedClicked(object sender, EventArgs e)
        {
            // Call to the Database to Update the Plan and Update InventoryItems
            var response = await App.APImanager.CompletePlan(_activePlan.PlanId);
            if (response.success)
            {
                await DisplayAlert("Success", "You cooked it!", "Okay");
                await Navigation.PopAsync();
            }
            else
            {
                // handle failure
                await DisplayAlert("Error", response.message, "Okay");
            }
        }
    }
}
