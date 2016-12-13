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
        public AddToPlanFromRecipePage()
        {
            InitializeComponent();
            // Instantiate Calendar and add to View
            SfCalendar calendar = new SfCalendar();
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
            var action = await DisplayActionSheet("Which Meal?", "Cancel", null, "Breakfast", "Lunch", "Dinner", "Snack", "Dessert");
            // TODO: Post this Meal Plan to WebAPI
            // TODO: On Success, pop this page from Navigation
        }
    }
}
