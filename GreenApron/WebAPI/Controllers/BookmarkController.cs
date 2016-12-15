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
        public async Task<JsonResult> AddBookmark([FromBody] BookmarkRequest bookmark)
        {
            // Check ModelState
            if (!ModelState.IsValid)
            {
                // If invalid, return error message
                return Json(new JsonResponse { success = false, message = "Something went wrong, please resubmit with all required fields." });
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
                return Json(new JsonResponse { success = false, message = "Something went wrong while saving your plan to the database, please try again." });
            }
            return Json( new JsonResponse { success = true, message = "Bookmark saved successfully"});
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
    }
}
