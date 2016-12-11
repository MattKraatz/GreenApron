using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GreenApron
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
			BindingContext = new LoginCreds();
        }

		private async void DoLogin(object sender, EventArgs e)
		{
			var creds = BindingContext;
			Navigation.InsertPageBefore(new HomePage(), this);
			await Navigation.PopAsync();
		}

		private async void DoRegistration(object sender, EventArgs e)
		{
			var user = (User)BindingContext;
			Navigation.InsertPageBefore(new HomePage(), this);
			await Navigation.PopAsync();
		}
    }
}
