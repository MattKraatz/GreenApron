using Xunit;

namespace Tests.WebAPI
{
    public class BookmarkTests
    {
        private TaskManager _task { get; set; } = new TaskManager();

        public BookmarkTests()
        {
        }

        [Fact]
        public async void CanAddBookmark()
        {
            var response = await _task.AddBookmark();
            Assert.NotNull(response);
            Assert.True(response.success);
            Assert.True(response.bookmarks.Count == 1);
            await _task.DeleteBookmark(response.bookmarks[0].BookmarkId);
        }

        [Fact]
        public async void CannotAddDupeBookmarks()
        {
            await _task.AddBookmark();
            var response = await _task.AddBookmark();
            Assert.NotNull(response);
            Assert.False(response.success);
            Assert.False(response.bookmarks.Count == 0);
            await _task.DeleteBookmark(response.bookmarks[0].BookmarkId);
        }

        [Fact]
        public async void CanGetBookmarks()
        {
            await _task.AddBookmark();
            var response = await _task.GetBookmarks();
            Assert.NotNull(response);
            Assert.True(response.success);
            Assert.True(response.bookmarks.Count > 0);
            await _task.DeleteBookmark(response.bookmarks[0].BookmarkId);
        }

        [Fact]
        public async void CanCheckExistentBookmark()
        {
            await _task.AddBookmark();
            var response = await _task.CheckBookmark();
            Assert.NotNull(response);
            Assert.True(response.success);
            Assert.True(response.bookmarks.Count == 1);
            await _task.DeleteBookmark(response.bookmarks[0].BookmarkId);
        }

        [Fact]
        public async void CanCheckNonexistentBookmark()
        {
            var response = await _task.CheckBookmark();
            Assert.NotNull(response);
            Assert.False(response.success);
        }

        [Fact]
        public async void CanDeleteBookmarks()
        {
            var mark = await _task.AddBookmark();
            var response = await _task.DeleteBookmark(mark.bookmarks[0].BookmarkId);
            Assert.NotNull(response);
            Assert.True(response.success);
        }
    }
}
