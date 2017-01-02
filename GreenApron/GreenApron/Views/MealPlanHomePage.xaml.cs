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
        PlanResponse response;

        public MealPlanHomePage()
        {
            InitializeComponent();
            GetMealPlans();
        }

        public async void GetMealPlans()
        {
            response = await App.APImanager.GetActivePlans();
            if (response.success)
            {
                // Print the page with meal plans
                PrintDays(response.plans);
            } else
            {
                await DisplayAlert("Error", response.message, "Okay");
                var addButton = new Button { Text = "Add a Plan", TextColor = Color.White, BackgroundColor = Color.FromHex("#50F75B") };
                addButton.Command = new Command(OnAddClicked);
                scrollStack.Children.Add(addButton);
            }
        }

        public async void OnAddClicked()
        {
            var tab = new TabbedPage();
            tab.Title = "New Plan";

            var page = new RecipeSearchPage();
            var page2 = new RecipeCollectionPage();

            tab.Children.Add(page);
            tab.Children.Add(page2);

            await Navigation.PushAsync(tab);
        }

        public async void OnAddClicked(DateTime date)
        {
            var tab = new TabbedPage();
            tab.Title = date.ToString("dddd, dd MMMM");

            var page = new RecipeSearchPage(date);
            var page2 = new RecipeCollectionPage(date);

            tab.Children.Add(page);
            tab.Children.Add(page2);

            await Navigation.PushAsync(tab);
        }

        public async void OnEditClicked(Plan plan)
        {
            var page = new PlanPage(plan);
            await Navigation.PushAsync(page);
        }
        
        //public async void OnCalendarTapped(object sender, Syncfusion.SfCalendar.XForms.CalendarTappedEventArgs args)
        //{
        //    selectedDate = args.datetime;
        //    var planNames = response.plans.Where(p => p.Date.Date == selectedDate.Date).Select(p => p.Meal + " - " + p.RecipeName).ToArray();
        //    var selectedMeal = await DisplayActionSheet("Which Meal?","Cancel",null,planNames);
        //    if (planNames.Contains(selectedMeal))
        //    {
        //        var plan = response.plans.FirstOrDefault(p => (p.Meal + " - " + p.RecipeName) == selectedMeal);
        //        // Grab recipe from Spoonacular
        //        plan.Recipe = await App.SpoonManager.GetRecipeByIdAsync(plan.RecipeId);
        //        // Instantiate new recipe page with plan as context
        //        var newPage = new PlanPage(plan);
        //        await Navigation.PushAsync(newPage);
        //    }
        //}
        
        public void PrintDays(List<Plan> plans)
        {
            for (var i = 0; i < 15; i ++)
            {
                var today = DateTime.Today.AddDays(i);

                // Create a label for the date
                var dateLabel = new Label
                {
                    Text = today.ToString("dddd, dd MMMM"),
                    TextColor = Color.White,
                    HorizontalTextAlignment = TextAlignment.Center,
                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                    BackgroundColor = Color.FromHex("#50F75B")
                };
                // Add padding
                var view = new ContentView() { Margin = new Thickness(0,5) };
                view.Content = dateLabel;
                // Add Date label to the stack
                scrollStack.Children.Add(view);

                var todayPlans = plans.Where(p => p.Date.Date == today.Date).ToList();
                if (todayPlans.Count > 0)
                {
                    PrintPlans(todayPlans, "Breakfast");
                    PrintPlans(todayPlans, "Lunch");
                    PrintPlans(todayPlans, "Snack");
                    PrintPlans(todayPlans, "Dinner");
                    PrintPlans(todayPlans, "Dessert");
                }
                var addButton = new Button { Text = "Add a Plan", TextColor = Color.White, BackgroundColor = Color.FromHex("#00DE0E"), HorizontalOptions = LayoutOptions.Center };
                addButton.Command = new Command<DateTime>(OnAddClicked);
                addButton.CommandParameter = today;
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
                                    new Image { HeightRequest = 50, WidthRequest = 80, Source = plan.RecipeImage }
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
    }
}
