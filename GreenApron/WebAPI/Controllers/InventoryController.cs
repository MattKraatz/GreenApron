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

        public InventoryController(GreenApronContext context)
        {
            _context = context;
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
            return Json(new InventoryResponse { success = true, message = "Grocery Item(s) retrieved successfully", InventoryItems = items });
        }

        // POST api/inventory/add
        // Adds a new inventory item record, and a new ingredient record if necessary
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
                }
                else
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
    }
}
