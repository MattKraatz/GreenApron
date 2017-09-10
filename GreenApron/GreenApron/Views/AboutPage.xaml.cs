using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace GreenApron.Views
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            AddTapGestures();
        }

        private void AddTapGestures()
        {
            var githubLabel_tap = new TapGestureRecognizer();
            githubLabel_tap.Tapped += (s, e) =>
            {
                Device.OpenUri(new Uri("https://github.com/MattKraatz/GreenApron"));
            };
            githubLink.GestureRecognizers.Add(githubLabel_tap);
        }
    }
}
