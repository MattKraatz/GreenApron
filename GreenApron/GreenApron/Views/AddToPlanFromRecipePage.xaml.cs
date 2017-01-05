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
        public Recipe selectedRecipe { get; set; }
        private SfCalendar _calendar { get; set; } = new SfCalendar {
            MinDate = DateTime.Today, MaxDate = DateTime.Today.AddDays(14),
            ShowInlineEvents = true
        };
        private MonthViewSettings _calendarSettings { get; set; } = new MonthViewSettings {
            TodayTextColor = Color.FromHex("#008A09"), InlineTextColor = Color.Black,
            InlineBackgroundColor = Color.FromHex("#FFC9C8"), SelectedDayTextColor = Color.FromHex("#008A09"),
            DateSelectionColor = Color.FromHex("#FFC9C8")
        };
        private CalendarEventCollection _collection { get; set; } = new CalendarEventCollection();

        public AddToPlanFromRecipePage(Recipe recipe)
        {
            InitializeComponent();
            selectedRecipe = recipe;
            // Instantiate Calendar and add to View
            _calendar.OnCalendarTapped += CacheSelectedDate;
            _calendar.MonthViewSettings = _calendarSettings;
            _calendar.VerticalOptions = LayoutOptions.StartAndExpand;
            layout.Children.Add(_calendar);
            // Instantiate Add to Plan button and add to View
            var addButton = new Button { Text = "Add to Plan", BackgroundColor = Color.FromHex("#00DE0E"),
                HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.CenterAndExpand,
                TextColor = Color.White };
            addButton.Clicked += OnAddToPlanClicked;
            layout.Children.Add(addButton);
            GetMealPlans();
        }

        public async void GetMealPlans()
        {
            var response = await App.APImanager.GetActivePlans();
            busy.IsRunning = false;
            busy.IsVisible = false;
            if (response.success)
            {
                // Add Appointments to the calendar
                foreach (Plan plan in response.plans)
                {
                    AddAppointment(plan);
                }
                _calendar.DataSource = _collection;
            }
        }

        public async void OnAddToPlanClicked(object sender, EventArgs e)
        {
            var mealStrings = new[] { "Breakfast", "Lunch", "Dinner", "Snack", "Dessert" };
			var button = (Button)sender;
			button.IsEnabled = false;
            var action = await DisplayActionSheet("Which Meal?", "Cancel", null, mealStrings);
            if (mealStrings.Contains(action))
			{
                // Post this Meal Plan to WebAPI
                var newPlan = new PlanRequest { userId = App.AuthManager.loggedInUser.UserId, date = selectedDate, meal = action, recipe = selectedRecipe };
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
			button.IsEnabled = true;
        }

        public void CacheSelectedDate(object sender, Syncfusion.SfCalendar.XForms.CalendarTappedEventArgs args)
        {
            selectedDate = args.datetime;
        }

        public void AddAppointment(Plan plan)
        {
            // Add a calendar appointment for each plan retrieved from the WebAPI
            CalendarInlineEvent appt = new CalendarInlineEvent();
            switch (plan.Meal)
            {
                case "Breakfast":
                    appt.StartTime = new DateTime(plan.Date.Year, plan.Date.Month, plan.Date.Day, 7, 0, 0);
                    appt.EndTime = new DateTime(plan.Date.Year, plan.Date.Month, plan.Date.Day, 8, 0, 0);
                    appt.Color = Color.FromHex("#FFC9C8");
                    break;
                case "Lunch":
                    appt.StartTime = new DateTime(plan.Date.Year, plan.Date.Month, plan.Date.Day, 11, 0, 0);
                    appt.EndTime = new DateTime(plan.Date.Year, plan.Date.Month, plan.Date.Day, 12, 0, 0);
                    appt.Color = Color.FromHex("#FF5652");
                    break;
                case "Snack":
                    appt.StartTime = new DateTime(plan.Date.Year, plan.Date.Month, plan.Date.Day, 14, 0, 0);
                    appt.EndTime = new DateTime(plan.Date.Year, plan.Date.Month, plan.Date.Day, 15, 0, 0);
                    appt.Color = Color.FromHex("#50F75B"); ;
                    break;
                case "Dinner":
                    appt.StartTime = new DateTime(plan.Date.Year, plan.Date.Month, plan.Date.Day, 18, 0, 0);
                    appt.EndTime = new DateTime(plan.Date.Year, plan.Date.Month, plan.Date.Day, 19, 0, 0);
                    appt.Color = Color.FromHex("#008A09"); ;
                    break;
                case "Dessert":
                    appt.StartTime = new DateTime(plan.Date.Year, plan.Date.Month, plan.Date.Day, 20, 0, 0);
                    appt.EndTime = new DateTime(plan.Date.Year, plan.Date.Month, plan.Date.Day, 21, 0, 0);
                    appt.Color = Color.FromHex("#AF0400"); ;
                    break;
            }
            appt.Subject = plan.Meal + " - " + plan.RecipeName;
            _collection.Add(appt);
        }
    }
}
