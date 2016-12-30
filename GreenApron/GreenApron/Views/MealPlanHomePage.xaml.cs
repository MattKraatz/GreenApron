using Syncfusion.SfCalendar.XForms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using System.Windows.Input;

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
            //calendar.OnCalendarTapped += OnCalendarTapped;
            //layout.Children.Add(calendar);
            GetMealPlans();
        }

        public async void GetMealPlans()
        {
            response = await App.APImanager.GetActivePlans();
            // Print the page with meal plans
            PrintDays(response.plans);
            //// Add Appointments to the calendar
            //foreach (Plan plan in response.plans)
            //{
            //    AddAppointment(plan);
            //}
            //calendar.DataSource = collection;
        }

        public async void OnAddaPlanClicked(object sender, EventArgs e)
        {
            var page = new RecipeSearchPage();
            await Navigation.PushAsync(page);
        }

        public async void OnAddClicked(DateTime date)
        {
            var page = new RecipeSearchPage();
            await Navigation.PushAsync(page);
        }

        public async void OnEditClicked(Plan plan)
        {
            var page = new PlanPage(plan);
            await Navigation.PushAsync(page);
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

        public void PrintDays(List<Plan> plans)
        {
            for (var i = 0; i < 30; i ++)
            {
                var today = DateTime.Today.AddDays(i);
                // Create a label for the date
                var dateLabel = new Label
                {
                    Text = today.ToString("dddd, dd MMMM"),
                    TextColor = Color.FromHex("#008A09"),
                    HorizontalTextAlignment = TextAlignment.Center,
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
                };
                // Add Date label to the stack
                scrollStack.Children.Add(dateLabel);

                var todayPlans = plans.Where(p => p.Date.Date == today.Date).ToList();
                if (todayPlans.Count > 0)
                {
                    PrintPlans(todayPlans, "Breakfast");
                    PrintPlans(todayPlans, "Lunch");
                    PrintPlans(todayPlans, "Snack");
                    PrintPlans(todayPlans, "Dinner");
                    PrintPlans(todayPlans, "Dessert");
                }
                var addButton = new Button { Text = "Add a Plan", TextColor = Color.White, BackgroundColor = Color.FromHex("#50F75B") };
                addButton.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => OnAddClicked(today)) });
                scrollStack.Children.Add(addButton);
            }
        }

        public void PrintPlans(List<Plan> plans, string meal)
        {
            var meals = plans.Where(p => p.Meal == meal).ToList();
            if (meals.Count > 0)
            {
                var mealLabel = new Label
                {
                    Text = meal,
                    TextColor = Color.FromHex("#AF0400"),
                    HorizontalTextAlignment = TextAlignment.Start
                };
                scrollStack.Children.Add(mealLabel);
                foreach (Plan plan in meals)
                {
                    var planCard = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Padding = new Thickness(20, 5),
                        Children =
                        {
                            new StackLayout
                            {
                                HorizontalOptions = LayoutOptions.Start,
                                VerticalOptions = LayoutOptions.Start,
                                Children =
                                {
                                    new Image { HeightRequest = 50, WidthRequest = 80, Source = "https://spoonacular.com/cdn/recipes_100x100/Creamy-Tomato-Tortellini-Soup-548458.jpg" }
                                }
                            },
                            new StackLayout
                            {
                                HorizontalOptions = LayoutOptions.StartAndExpand,
                                VerticalOptions = LayoutOptions.StartAndExpand,
                                Children =
                                {
                                    new Label { Text = plan.RecipeName, VerticalOptions = LayoutOptions.Start },
                                    new Label { Text = "tap to view"}
                                }
                            }
                        }
                    };
                    planCard.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => OnEditClicked(plan)) });
                    scrollStack.Children.Add(planCard);
                }
            }
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
                    appt.Color = Color.Yellow;
                    break;
                case "Lunch":
                    appt.StartTime = new DateTime(plan.Date.Year, plan.Date.Month, plan.Date.Day, 11, 0, 0);
                    appt.EndTime = new DateTime(plan.Date.Year, plan.Date.Month, plan.Date.Day, 12, 0, 0);
                    appt.Color = Color.Blue;
                    break;
                case "Snack":
                    appt.StartTime = new DateTime(plan.Date.Year, plan.Date.Month, plan.Date.Day, 14, 0, 0);
                    appt.EndTime = new DateTime(plan.Date.Year, plan.Date.Month, plan.Date.Day, 15, 0, 0);
                    appt.Color = Color.Olive;
                    break;
                case "Dinner":
                    appt.StartTime = new DateTime(plan.Date.Year, plan.Date.Month, plan.Date.Day, 18, 0, 0);
                    appt.EndTime = new DateTime(plan.Date.Year, plan.Date.Month, plan.Date.Day, 19, 0, 0);
                    appt.Color = Color.Red;
                    break;
                case "Dessert":
                    appt.StartTime = new DateTime(plan.Date.Year, plan.Date.Month, plan.Date.Day, 20, 0, 0);
                    appt.EndTime = new DateTime(plan.Date.Year, plan.Date.Month, plan.Date.Day, 21, 0, 0);
                    appt.Color = Color.Teal;
                    break;
            }
            appt.Subject = plan.Meal + " - " + plan.RecipeName;
            collection.Add(appt);
        }
    }
}
