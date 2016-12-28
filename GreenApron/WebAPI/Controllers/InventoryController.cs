using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class InventoryController : Controller
    {
        private GreenApronContext _context { get; set; }
        private IngredientManager _ingManager { get; set; }

        public InventoryController(GreenApronContext context)
        {
            _context = context;
            _ingManager = new IngredientManager(_context);
        }

        // GET api/inventory/getall/{userId}
        // Returns all active inventory items
        [HttpGet("{userId}")]
        public async Task<JsonResult> GetAll([FromRoute] Guid userId)
        {
            var items = await _context.InventoryItem.Where(ii => ii.Amount > 0).Where(ii => ii.UserId == userId).Include(ii => ii.Ingredient).ToListAsync();
            if (items.Count < 1)
            {
                return Json(new InventoryResponse { success = false, message = "No inventory items were found, have you added any? " });
            }
            return Json(new InventoryResponse { success = true, message = "Inventory Item(s) retrieved successfully", InventoryItems = items });
        }

        // POST api/inventory/add
        // Adds a new inventory item record, and a new ingredient record if necessary
        [HttpPost("{userId}")]
        public async Task<JsonResult> Add([FromBody] extIngredient item, [FromRoute] Guid userId)
        {
            // Find an existing ingredient item record in the database by Id or by name
            var dbIngredient = await _ingManager.CheckDB(item);
            Ingredient newIngredient = null;
            if (dbIngredient.IngredientId == -1)
            {
                return Json(new JsonResponse { success = false, message = "Something went wrong while saving an ingredient to the database, please try again." });
            }
            // Add a inventory item record to the database
            var newInventoryItem = new InventoryItem { Amount = item.amount, Unit = item.unit, UserId = userId };
            if (newIngredient != null)
            {
                newInventoryItem.IngredientId = newIngredient.IngredientId;
            }
            else
            {
                newInventoryItem.IngredientId = item.id;
            }
            _context.InventoryItem.Add(newInventoryItem);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return Json(new JsonResponse { success = false, message = "Something went wrong while saving to the database, please try again." });
            }
            return Json(new JsonResponse { success = true, message = "Grocery Item added successfully." });
        }

        // POST api/inventory/update
        // Updates inventory items
        [HttpPost]
        public async Task<JsonResult> Update([FromBody] InventoryRequest request)
        {
            foreach (InventoryItem item in request.items)
            {
                // Find the inventory item record in the database, update amount and unit properties
                var dbItem = await _context.InventoryItem.SingleOrDefaultAsync(ii => ii.InventoryItemId == item.InventoryItemId);
                if (dbItem != null)
                {
                    dbItem.Amount = item.Amount;
                    dbItem.Unit = item.Unit;
                    _context.Entry(dbItem).State = EntityState.Modified;
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

        // GET api/inventory/delete/{id}
        // Deletes inventory item
        [HttpGet("{id}")]
        public async Task<JsonResult> Delete([FromRoute] Guid id)
        {
            // Find the inventory item record in the database
            var dbItem = await _context.InventoryItem.SingleOrDefaultAsync(ii => ii.InventoryItemId == id);
            if (dbItem != null)
            {
                _context.InventoryItem.Remove(dbItem);
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
