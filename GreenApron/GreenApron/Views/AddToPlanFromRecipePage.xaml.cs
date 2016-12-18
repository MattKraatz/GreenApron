using Syncfusion.SfCalendar.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace GreenApron
{
    public partial class AddToPlanFromRecipePage : ContentPage
    {
        public DateTime selectedDate { get; set; }

        public AddToPlanFromRecipePage()
        {
            InitializeComponent();
            // Instantiate Calendar and add to View
            SfCalendar calendar = new SfCalendar();
            calendar.OnCalendarTapped += CacheSelectedDate;
            layout.Children.Add(calendar);
            // Instantiate Add to Plan button and add to View
            var addButton = new Button { Text = "Add to Plan", BackgroundColor = Color.FromHex("#77D065"), TextColor = Color.White };
            addButton.Clicked += OnAddToPlanClicked;
            layout.Children.Add(addButton);
            // TODO: Retrieve Active Meal Plans from WebAPI
            // TODO: Attach Active Meal Plans to the Calendar
        }

        public async void OnAddToPlanClicked(object sender, EventArgs e)
        {
            var mealStrings = new[] { "Breakfast", "Lunch", "Dinner", "Snack", "Dessert" };
            var action = await DisplayActionSheet("Which Meal?", "Cancel", null, mealStrings);
            if (mealStrings.Contains(action))
            {
                // Post this Meal Plan to WebAPI
                var newPlan = new PlanRequest { userId = App.AuthManager.loggedInUser.UserId, date = selectedDate, meal = action, recipe = App.SpoonManager.selectedRecipe };
                JsonResponse response = await App.APImanager.AddPlan(newPlan);
                // On Success, pop this page from Navigation
                if (response.success)
                {
                    await Navigation.PopAsync();
                }
                else
                {
                    // handle failure
                    await DisplayAlert("Error", response.message, "Okay");
                }
            }
        }

        public void CacheSelectedDate(object sender, Syncfusion.SfCalendar.XForms.CalendarTappedEventArgs args)
        {
            selectedDate = args.datetime;
        }
    }
}
