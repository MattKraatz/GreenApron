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
        Random rand = new Random();

        public GroceryController(GreenApronContext context)
        {
            _context = context;
        }

        // GET api/grocery/getall/{userId}
        // Returns all active grocery items
        [HttpGet("{userId}")]
        public async Task<JsonResult> GetAll([FromRoute] Guid userId)
        {
            var items = await _context.GroceryItem.Where(gi => gi.DateCompleted == null).Include(gi => gi.Ingredient).ToListAsync();
            if (items.Count < 1)
            {
                return Json(new GroceryResponse { success = false, message = "No grocery items were found, have you added any? " });
            }
            return Json(new GroceryResponse { success = true, message = "Grocery Item(s) retrieved successfully", GroceryItems = items });
        }

        // POST api/grocery/update
        // Updates grocery items that were purchased
        [HttpPost]
        public async Task<JsonResult> Update([FromBody] GroceryRequest request)
        {
            foreach (GroceryItem item in request.items)
            {
                // Find the grocery item record in the database, update it's DateCompleted property
                var dbItem = await _context.GroceryItem.SingleOrDefaultAsync(gi => gi.GroceryItemId == item.GroceryItemId);
                if (dbItem != null)
                {
                    dbItem.DateCompleted = item.DateCompleted;
                    dbItem.Amount = item.Amount;
                    dbItem.Unit = item.Unit;
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

        // POST api/grocery/add
        // Adds a new grocery item record, and a new ingredient record if necessary
        [HttpPost("{userId}")]
        public async Task<JsonResult> Add([FromBody] IngredientRequest item, [FromRoute] Guid userId)
        {
            // Find an existing ingredient item record in the database by Id or by name
            var dbIngredient = await _context.Ingredient.SingleOrDefaultAsync(i => i.IngredientId == item.id || i.IngredientName == item.name);
            Ingredient newIngredient = null;
            if (dbIngredient == null)
            {
                // If no ingredient record is found, add a new one
                newIngredient = new Ingredient() { Aisle = item.aisle, ImageURL = item.image, IngredientName = item.name };
                if (item.id < 1)
                {
                    newIngredient.IngredientId = rand.Next(100000000, 999999999);
                } else
                {
                    newIngredient.IngredientId = item.id;
                }
                _context.Ingredient.Add(newIngredient);
                _context.Database.OpenConnection();
                try
                {
                    // Execute Identity Insert command to inject Spoonacular's primary key
                    await _context.Database.ExecuteSqlCommandAsync("SET IDENTITY_INSERT dbo.Ingredient ON");
                    await _context.SaveChangesAsync();
                    await _context.Database.ExecuteSqlCommandAsync("SET IDENTITY_INSERT dbo.Ingredient OFF");
                }
                catch
                {
                    return Json(new JsonResponse { success = false, message = "Something went wrong while saving an ingredient to the database, please try again." });
                }
                finally
                {
                    _context.Database.CloseConnection();
                }
            }
            // Add a grocery item record to the database
            var newGroceryItem = new GroceryItem { Amount = item.amount, Unit = item.unit, UserId = userId };
            if (newIngredient != null)
            {
                newGroceryItem.IngredientId = newIngredient.IngredientId;
            } else
            {
                newGroceryItem.IngredientId = item.id;
            }
            _context.GroceryItem.Add(newGroceryItem);
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
    }
}
