using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GreenApron
{
    public partial class LoginPage : TabbedPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new User();
        }

		private async void DoLogin(object sender, EventArgs e)
		{
			var creds = BindingContext;
			Navigation.InsertPageBefore(new HomePage(), this);
			await Navigation.PopAsync();
		}

		private async void DoRegistration(object sender, EventArgs e)
		{
			var user = BindingContext;
			Navigation.InsertPageBefore(new HomePage(), this);
			await Navigation.PopAsync();
		}
    }
}
