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
            var user = await _task.RegisterUser();
            var response = await _task.AddBookmark(user.user.UserId);
            Assert.NotNull(response);
            Assert.True(response.success);
            Assert.True(response.bookmarks.Count == 1);
            await _task.DeleteBookmark(response.bookmarks[0].BookmarkId);
            await _task.DeleteUser();
        }

        [Fact]
        public async void CannotAddDupeBookmarks()
        {
            var user = await _task.RegisterUser();
            await _task.AddBookmark(user.user.UserId);
            var response = await _task.AddBookmark(user.user.UserId);
            Assert.NotNull(response);
            Assert.False(response.success);
            Assert.False(response.bookmarks.Count == 0);
            await _task.DeleteBookmark(response.bookmarks[0].BookmarkId);
            await _task.DeleteUser();
        }

        [Fact]
        public async void CanGetBookmarks()
        {
            var user = await _task.RegisterUser();
            await _task.AddBookmark(user.user.UserId);
            var response = await _task.GetBookmarks(user.user.UserId);
            Assert.NotNull(response);
            Assert.True(response.success);
            Assert.True(response.bookmarks.Count > 0);
            await _task.DeleteBookmark(response.bookmarks[0].BookmarkId);
            await _task.DeleteUser();
        }

        [Fact]
        public async void CanCheckExistentBookmark()
        {
            var user = await _task.RegisterUser();
            await _task.AddBookmark(user.user.UserId);
            var response = await _task.CheckBookmark(user.user.UserId);
            Assert.NotNull(response);
            Assert.True(response.success);
            Assert.True(response.bookmarks.Count == 1);
            await _task.DeleteBookmark(response.bookmarks[0].BookmarkId);
            await _task.DeleteUser();
        }

        [Fact]
        public async void CanCheckNonexistentBookmark()
        {
            var user = await _task.RegisterUser();
            var response = await _task.CheckBookmark(user.user.UserId);
            Assert.NotNull(response);
            Assert.False(response.success);
            await _task.DeleteUser();
        }

        [Fact]
        public async void CanDeleteBookmarks()
        {
            var user = await _task.RegisterUser();
            var mark = await _task.AddBookmark(user.user.UserId);
            var response = await _task.DeleteBookmark(mark.bookmarks[0].BookmarkId);
            Assert.NotNull(response);
            Assert.True(response.success);
            await _task.DeleteUser();
        }
    }
}
