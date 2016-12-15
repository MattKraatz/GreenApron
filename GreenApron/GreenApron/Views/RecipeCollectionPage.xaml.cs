using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace GreenApron
{
    public partial class RecipeCollectionPage : ContentPage
    {
        public ObservableCollection<Bookmark> bookmarkItems { get; private set; } = new ObservableCollection<Bookmark>();

        public RecipeCollectionPage()
        {
            InitializeComponent();
            bookmarkList.ItemsSource = bookmarkItems;
            GetBookmarks();
        }

        public async void GetBookmarks()
        {
            var bookmarks = await App.APImanager.GetBookmarks();
            if (bookmarks.success)
            {
                foreach (Bookmark item in bookmarks.bookmarks)
                {
                    bookmarkItems.Add(item);
                }
            }
            else
            {
                await DisplayAlert("Error", bookmarks.message, "Okay");
            }
        }

        public void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            DisplayAlert("Alert", "You tapped a bookmark dude.", "Okay");
        }
    }
}
