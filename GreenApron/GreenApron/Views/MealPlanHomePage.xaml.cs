using Syncfusion.SfCalendar.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace GreenApron
{
    public partial class MealPlanHomePage : ContentPage
    {
        CalendarEventCollection collection = new CalendarEventCollection();
        SfCalendar calendar = new SfCalendar();
        DateTime selectedDate;
        PlanResponse response;

        public MealPlanHomePage()
        {
            InitializeComponent();
            calendar.OnCalendarTapped += OnCalendarTapped;
            layout.Children.Add(calendar);
            GetMealPlans();
        }

        public async void GetMealPlans()
        {
            response = await App.APImanager.GetActivePlans();
            // Add a calendar appointment for each plan retrieved from the WebAPI
            foreach (Plan plan in response.plans)
            {
                CalendarInlineEvent events = new CalendarInlineEvent();
                switch(plan.Meal)
                {
                    case "Breakfast":
                        events.StartTime = new DateTime(plan.Date.Year, plan.Date.Month, plan.Date.Day, 7, 0, 0);
                        events.EndTime = new DateTime(plan.Date.Year, plan.Date.Month, plan.Date.Day, 8, 0, 0);
                        events.Color = Color.Yellow;
                        break;
                    case "Lunch":
                        events.StartTime = new DateTime(plan.Date.Year, plan.Date.Month, plan.Date.Day, 11, 0, 0);
                        events.EndTime = new DateTime(plan.Date.Year, plan.Date.Month, plan.Date.Day, 12, 0, 0);
                        events.Color = Color.Blue;
                        break;
                    case "Snack":
                        events.StartTime = new DateTime(plan.Date.Year, plan.Date.Month, plan.Date.Day, 14, 0, 0);
                        events.EndTime = new DateTime(plan.Date.Year, plan.Date.Month, plan.Date.Day, 15, 0, 0);
                        events.Color = Color.Olive;
                        break;
                    case "Dinner":
                        events.StartTime = new DateTime(plan.Date.Year, plan.Date.Month, plan.Date.Day, 18, 0, 0);
                        events.EndTime = new DateTime(plan.Date.Year, plan.Date.Month, plan.Date.Day, 19, 0, 0);
                        events.Color = Color.Red;
                        break;
                    case "Dessert":
                        events.StartTime = new DateTime(plan.Date.Year, plan.Date.Month, plan.Date.Day, 20, 0, 0);
                        events.EndTime = new DateTime(plan.Date.Year, plan.Date.Month, plan.Date.Day, 21, 0, 0);
                        events.Color = Color.Teal;
                        break;
                }
                events.Subject = plan.Meal + " - " + plan.RecipeName;
                collection.Add(events);
            }
            calendar.DataSource = collection;
        }

        public async void OnCalendarTapped(object sender, Syncfusion.SfCalendar.XForms.CalendarTappedEventArgs args)
        {
            selectedDate = args.datetime;
            var planNames = response.plans.Where(p => p.Date.Date == selectedDate.Date).Select(p => p.Meal + " - " + p.RecipeName).ToArray();
            var selectedMeal = await DisplayActionSheet("Which Meal?","Cancel",null,planNames);
            if (planNames.Contains(selectedMeal))
            {
                var plan = response.plans.FirstOrDefault(p => (p.Meal + " - " + p.RecipeName) == selectedMeal);
                // Grab recipe from Spoonacular
                plan.Recipe = await App.SpoonManager.GetRecipeByIdAsync(plan.RecipeId);
                // Instantiate new recipe page with plan as context
                var newPage = new PlanPage(plan);
                await Navigation.PushAsync(newPage);
            }
        }
    }
}
