using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace GreenApron
{
    public partial class RecipePage : ContentPage
    {
        public RecipePage()
        {
            InitializeComponent();
        }

        public async void OnAddToPlanClicked(object sender, EventArgs e)
        {
            // Add a AddToPlanFromRecipePage instance to the navigation stack
            var page = new AddToPlanFromRecipePage();
            await Navigation.PushAsync(page);
        }
    }
}
