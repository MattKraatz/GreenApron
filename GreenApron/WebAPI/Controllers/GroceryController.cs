using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI
{
    public class GroceryController : Controller
    {
        private GreenApronContext _context { get; set; }

        public GroceryController(GreenApronContext context)
        {
            _context = context;
        }

        // GET api/grocery/getall/{userId}
        // Returns all active grocery items
        [HttpGet("{userId}")]
        public async Task<GroceryResponse> GetAll([FromRoute] Guid userId)
        {
            var items = await _context.GroceryItem.Where(gi => gi.DateCompleted != null).ToListAsync();
            if (items.Count < 1)
            {
                return new GroceryResponse { success = false, message = "No grocery items were found, have you added any? " };
            }
            return new GroceryResponse { success = true, message = "Grocery Item(s) retrieved successfully", GroceryItems = items };
        }
    }
}
