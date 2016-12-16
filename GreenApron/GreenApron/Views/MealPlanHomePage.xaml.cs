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
            calendar.ShowInlineEvents = true;
            calendar.OnCalendarTapped += OnCalendarTapped;
            layout.Children.Add(calendar);
            GetMealPlans();
        }

        public async void GetMealPlans()
        {
            response = await App.APImanager.GetActivePlans();
            foreach (Plan plan in response.plans)
            {
                CalendarInlineEvent events = new CalendarInlineEvent();
                events.StartTime = plan.Date;
                events.EndTime = plan.Date;
                events.Subject = plan.Meal + ": " + plan.RecipeName;
                collection.Add(events);
            }
            calendar.DataSource = collection;
        }

        public async void OnCalendarTapped(object sender, Syncfusion.SfCalendar.XForms.CalendarTappedEventArgs args)
        {
            selectedDate = args.datetime;
            var planNames = response.plans.Where(p => p.Date.Date == selectedDate.Date).Select(p => p.Meal + ": " + p.RecipeName).ToArray();
            var selectedMeal = await DisplayActionSheet("Which Meal?","Cancel","Go",planNames);
            var plan = response.plans.SingleOrDefault(p => (p.Meal + ": " + p.RecipeName) == selectedMeal);
            // Grab recipe from Spoonacular
            plan.Recipe = await App.SpoonManager.GetRecipeByIdAsync(plan.RecipeId);
            // Instantiate new recipe page with plan as context
            var newPage = new PlanPage(plan);
            await Navigation.PushAsync(newPage);
        }
    }
}
