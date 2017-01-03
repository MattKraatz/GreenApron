using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI
{
    [Route("api/[controller]/[action]")]
    public class BookmarkController : Controller
    {
        private GreenApronContext _context { get; set; }

        public BookmarkController(GreenApronContext context)
        {
            _context = context;
        }

        // POST api/bookmark/addbookmark
        [HttpPost]
        public async Task<BookmarkResponse> AddBookmark([FromBody] BookmarkRequest bookmark)
        {
            // Check ModelState
            if (!ModelState.IsValid)
            {
                // If invalid, return error message
                return new BookmarkResponse { success = false, message = "Something went wrong, please resubmit with all required fields." };
            }
            // Check for existing bookmarks first
            var check = await Check(bookmark);
            if (check.success)
            {
                return new BookmarkResponse { success = false, message = "Already bookmarked.", bookmarks = check.bookmarks };
            }
            // Create Bookmark record and save to database
            var newBookmark = new Bookmark { UserId = bookmark.userId, RecipeId = bookmark.RecipeId, Title = bookmark.Title, ImageURL = bookmark.ImageURL };
            _context.Bookmark.Add(newBookmark);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return new BookmarkResponse { success = false, message = "Something went wrong while saving your plan to the database, please try again." };
            }
            var response = new BookmarkResponse { success = true, message = "Bookmark saved successfully" };
            response.bookmarks = new List<Bookmark>();
            response.bookmarks.Add(newBookmark);
            return response;
        }

        // GET api/bookmark/{uid}
        [HttpGet("{userId}")]
        public async Task<BookmarkResponse> GetAll([FromRoute] Guid userId)
        {
            var bookmarks = await _context.Bookmark.Where(b => b.UserId == userId).ToListAsync();
            if (bookmarks.Count < 1)
            {
                return new BookmarkResponse { success = false, message = "No bookmarks were found, have you added any?" };
            }
            return new BookmarkResponse { success = true, message = "Bookmark(s) retrieved successfully.", bookmarks = bookmarks};
        }

        // POST api/bookmark/check
        [HttpPost]
        public async Task<BookmarkResponse> Check([FromBody] BookmarkRequest bookmark)
        {
            var check = await _context.Bookmark.Where(b => b.UserId == bookmark.userId).Where(b => b.RecipeId == bookmark.RecipeId).ToListAsync();
            if (check.Count > 0)
            {
                return new BookmarkResponse { success = true, message = "Already bookmarked.", bookmarks = check };
            }
            return new BookmarkResponse { success = false, message = "Not bookmarked. " };
        }

        // GET api/bookmark/delete
        [HttpGet("{id}")]
        public async Task<JsonResponse> Delete([FromRoute] Guid id)
        {
            // Find the bookmark record in the database
            var dbItem = await _context.Bookmark.SingleOrDefaultAsync(b => b.BookmarkId == id);
            if (dbItem != null)
            {
                _context.Bookmark.Remove(dbItem);
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return new JsonResponse { success = false, message = "Something went wrong while saving to the database, please try again." };
            }
            return new JsonResponse { success = true, message = "Database updated successfully." };
        }
    }
}
