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
				busy.IsRunning = false;
				busy.IsVisible = false;
			}
			else
			{
				busy.IsRunning = false;
				busy.IsVisible = false;

				var view = new StackLayout { Padding = new Thickness(20), Spacing = 20, VerticalOptions = LayoutOptions.CenterAndExpand };
				var label = new Label { Text = "Hmm, doesn't look like you have any plans yet, wanna add one?", FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
					VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalTextAlignment = TextAlignment.Center, Opacity = 0.75 };
                var addButton = new Button { Text = "Add a Plan", TextColor = Color.White, BackgroundColor = Color.FromHex("#50F75B"),
					HorizontalOptions = LayoutOptions.FillAndExpand };
                addButton.Command = new Command(OnAddClicked);

				view.Children.Add(label);
                view.Children.Add(addButton);

				scrollStack.Children.Add(view);
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
        
        public void PrintDays(List<Plan> plans)
        {
            for (var i = 0; i < 15; i ++)
            {
                var today = DateTime.Today.AddDays(i);

                // Create a label for the date
                var dateLabel = new Label
                {
                    Text = today.ToString("dddd, MMMM d"),
                    TextColor = Color.Black,
                    HorizontalTextAlignment = TextAlignment.Center,
					FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
                };
				// Add padding and margin to a view container
				var view = new ContentView() { BackgroundColor = Color.FromHex("#00DE0E"), Padding = new Thickness(0, 10), Margin = new Thickness(0, 10) };
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
				else
				{
					// Create a label for the default text if no plans are available for that date
					var label = new Label
					{
						Text = "No plans for this date yet, want to add one?",
						TextColor = Color.Black,
						Opacity = 0.5,
						HorizontalTextAlignment = TextAlignment.Center,
						FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
					};
					scrollStack.Children.Add(label);
				}
                var addButton = new Button { Text = "Add a Plan", TextColor = Color.White, BackgroundColor = Color.FromHex("#50F75B"), HorizontalOptions = LayoutOptions.Center, WidthRequest = 150 };
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
					Margin = new Thickness(15, 0),
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
                                    new Image { HeightRequest = 50, WidthRequest = 60, Source = plan.RecipeImage }
                                }
                            },
                            new StackLayout
                            {
                                HorizontalOptions = LayoutOptions.StartAndExpand,
                                VerticalOptions = LayoutOptions.StartAndExpand,
                                Children =
                                {
                                    new Label { Text = plan.RecipeName, VerticalOptions = LayoutOptions.Start, LineBreakMode = LineBreakMode.TailTruncation },
                                    new Label { Text = "tap to view", Opacity = 0.5 }
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
