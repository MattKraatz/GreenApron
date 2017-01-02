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
        private DateTime? _activeDate { get; set; }

        public RecipeCollectionPage()
        {
            InitializeComponent();
            bookmarkList.ItemsSource = bookmarkItems;
            busy.IsVisible = true;
            busy.IsRunning = true;
            GetBookmarks();
            busy.IsVisible = false;
            busy.IsRunning = false;
        }
        
        public RecipeCollectionPage(DateTime activeDate)
        {
            InitializeComponent();
            bookmarkList.ItemsSource = bookmarkItems;
            _activeDate = activeDate;
            busy.IsVisible = true;
            busy.IsRunning = true;
            GetBookmarks();
            busy.IsVisible = false;
            busy.IsRunning = false;
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
            var bookmark = e.Item as Bookmark;
            var recipePage = new RecipePage(bookmark.RecipeId, _activeDate);
            Navigation.PushAsync(recipePage);
            // Deselect the tapped item
            bookmarkList.SelectedItem = null;
        }
    }
}
