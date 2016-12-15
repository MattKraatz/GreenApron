using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI
{
    [Route("api/[controller]/[action]")]
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
        public async Task<JsonResult> GetAll([FromRoute] Guid userId)
        {
            var items = await _context.GroceryItem.Where(gi => gi.DateCompleted != null).ToListAsync();
            if (items.Count < 1)
            {
                return Json(new GroceryResponse { success = false, message = "No grocery items were found, have you added any? " });
            }
            return Json(new GroceryResponse { success = true, message = "Grocery Item(s) retrieved successfully", GroceryItems = items });
        }

        // POST api/grocery/update
        // Updates grocery items that were purchased
        public async Task<JsonResult> Update([FromBody] GroceryRequest request)
        {
            if (!ModelState.IsValid)
            {
                return Json(new JsonResponse { success = false, message = "Something went wrong, please resubmit with all required fields." });
            }
            foreach (GroceryItem item in request.items)
            {
                // Find the grocery item record in the database, update it's DateCompleted property
                var dbItem = await _context.GroceryItem.SingleOrDefaultAsync(gi => gi.GroceryItemId == item.GroceryItemId);
                if (dbItem != null)
                {
                    dbItem.DateCompleted = item.DateCompleted;
                    _context.Entry(dbItem).State = EntityState.Modified;
                }
                // Look for an existing inventory item for the same ingredient
                var dbInventoryItem = await _context.InventoryItem.SingleOrDefaultAsync(ii => ii.IngredientId == item.IngredientId);
                if (dbInventoryItem != null)
                {
                    dbInventoryItem.Amount += item.Amount;
                    _context.Entry(dbInventoryItem).State = EntityState.Modified;
                // Otherwise add a new inventory item for this ingredient
                } else
                {
                    var newInventoryItem = new InventoryItem { IngredientId = item.IngredientId, UserId = item.UserId, Amount = item.Amount, Unit = item.Unit };
                    _context.InventoryItem.Add(newInventoryItem);
                }
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return Json(new JsonResponse { success = false, message = "Something went wrong while saving to the database, please try again." });
            }
            return Json(new JsonResponse { success = true, message = "Database updated successfully." });
        }
    }
}
