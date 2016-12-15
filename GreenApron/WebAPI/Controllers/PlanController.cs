using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI
{
    [Route("api/[controller]/[action]")]
    public class PlanController : Controller
    {
        private GreenApronContext _context { get; set; }
        Random rand = new Random();

        public PlanController(GreenApronContext context)
        {
            _context = context;
        }

        // POST api/plan/addplan
        [HttpPost]
        public async Task<JsonResult> AddPlan([FromBody] PlanRequest plan)
        {
            // Check ModelState
            if (!ModelState.IsValid)
            {
                // If invalid, return error message
                return Json(new JsonResponse { success = false, message = "Something went wrong, please resubmit with all required fields." });
            }
            // Create Plan record and save to database
            var newPlan = new Plan { Date = plan.date, Meal = plan.meal, ServingsYield = plan.servingsYield, RecipeName = plan.recipe.title, RecipeId = plan.recipe.id, UserId = plan.userId};
            _context.Plan.Add(newPlan);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return Json(new JsonResponse { success = false, message = "Something went wrong while saving your plan to the database, please try again." });
            }

            // Retrieve all active plans to be used in the following loop
            var activePlans = await _context.Plan.Where(p => DateTime.Compare(p.Date, DateTime.Now) >= 0).Select(p => p.PlanId).ToListAsync();
            // Loop over recipe ingredients and add grocery items for each
            foreach (extIngredient planIngredient in plan.recipe.extendedIngredients)
            {
                var testy = planIngredient;
                Ingredient newIngredient = new Ingredient();
                // Add this ingredient to the database, if it doesn't already exist
                var dbIngredient = await _context.Ingredient.SingleOrDefaultAsync(i => i.IngredientId == planIngredient.id);
                if (dbIngredient == null)
                {
                    if (planIngredient.id < 1)
                    {
                        newIngredient.IngredientId = rand.Next(100000000, 999999999);
                    }
                    else
                    {
                        newIngredient.IngredientId = planIngredient.id;
                    }
                    newIngredient.IngredientName = planIngredient.name;
                    newIngredient.Aisle = planIngredient.aisle;
                    newIngredient.ImageURL = planIngredient.image;
                    _context.Ingredient.Add(newIngredient);
                    _context.Database.OpenConnection();
                    try
                    {
                        await _context.Database.ExecuteSqlCommandAsync("SET IDENTITY_INSERT dbo.Ingredient ON");
                        await _context.SaveChangesAsync();
                        await _context.Database.ExecuteSqlCommandAsync("SET IDENTITY_INSERT dbo.Ingredient OFF");
                    }
                    catch
                    {
                        return Json(new JsonResponse { success = false, message = "Something went wrong while saving your grocery items to the database, please try again." });
                    }
                    finally
                    {
                        _context.Database.CloseConnection();
                    }
                };
                // Add a PlanIngredient record for this ingredient
                var newPlanIngredient = new PlanIngredient { PlanId = newPlan.PlanId, Amount = planIngredient.amount, unit = planIngredient.unit };
                if (newIngredient.IngredientId > 0)
                {
                    newPlanIngredient.IngredientId = newIngredient.IngredientId;
                } else
                {
                    newPlanIngredient.IngredientId = dbIngredient.IngredientId;
                }
                _context.PlanIngredient.Add(newPlanIngredient);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    return Json(new JsonResponse { success = false, message = "Something went wrong while saving your grocery items to the database, please try again." });
                }
                // Sum up total requirement for this ingredient for all active plans
                // Currently assumes all ingredients share the same unit of measurement
                var totalRequirement = await _context.PlanIngredient.Where(pi => _context.Plan.Where(p => DateTime.Compare(p.Date, DateTime.Now) >= 0).SingleOrDefault(p => p.PlanId == pi.PlanId) != null).Where(pi => pi.IngredientId == planIngredient.id).SumAsync(pi => pi.Amount);
                // Sum up total existing inventory for this ingredient
                var totalInventory = await _context.InventoryItem.Where(ii => ii.IngredientId == planIngredient.id).SumAsync(ii => ii.Amount);
                if (totalInventory < totalRequirement)
                {
                    var groceryQty = totalRequirement - totalInventory;
                    // Find an existing GroceryItem record for this ingredient, if it exists, modify it with the new quantity
                    var groceryItem = await _context.GroceryItem.SingleOrDefaultAsync(gi => gi.IngredientId == planIngredient.id);
                    if (groceryItem != null)
                    {
                        groceryItem.Amount = groceryQty;
                        _context.Entry(groceryItem).State = EntityState.Modified;
                    }
                    // Else add a new GroceryItem record
                    else
                    {
                        var newGroceryItem = new GroceryItem { IngredientId = planIngredient.id, Amount = totalRequirement, Unit = planIngredient.unit, UserId = plan.userId };
                        _context.GroceryItem.Add(newGroceryItem);
                    }
                }
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return Json(new JsonResponse { success = false, message = "Something went wrong while saving your grocery items to the database, please try again." });
            }

            return Json( new JsonResponse { success = true, message = "Plan saved successfully"});
        }

        // GET api/plan/getall/{userId}
        // Returns all active plans
        [HttpGet("{userId}")]
        public async Task<JsonResult> GetAll([FromRoute] Guid userId)
        {
            var plans = await _context.Plan.Where(p => p.UserId == userId).ToListAsync();
            if (plans.Count < 1)
            {
                return Json(new PlanResponse { success = false, message = "No plans were found, have you added any?" });
            }
            return Json(new PlanResponse { success = true, message = "Plan(s) retrieved successfully.", plans = plans });
        }

        // POST api/plan/complete/{planId}
        // Completes a plan, deducts amounts from inventory items
        [HttpPost("{planId}")]
        public async Task<JsonResult> Complete([FromRoute] Guid planId)
        {
            var plan = await _context.Plan.SingleOrDefaultAsync(p => p.PlanId == planId);
            if (plan == null)
            {
                return Json(new JsonResponse { success = false, message = "Something went wrong, please refresh your client." });
            }
            plan.DateCompleted = DateTime.Now;
            _context.Entry(plan).State = EntityState.Modified;
            var planIngredients = await _context.PlanIngredient.Where(pi => pi.PlanId == plan.PlanId).ToListAsync();
            foreach (PlanIngredient ingredient in planIngredients)
            {
                var inventoryItem = await _context.InventoryItem.SingleOrDefaultAsync(ii => ii.IngredientId == ingredient.IngredientId);
                if (inventoryItem != null)
                {
                    inventoryItem.Amount -= ingredient.Amount;
                    if (inventoryItem.Amount < 0)
                    {
                        inventoryItem.Amount = 0;
                    }
                    _context.Entry(inventoryItem).State = EntityState.Modified;
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
            return Json(new JsonResponse { success = true, message = "Plan updated successfully" });
        }
    }
}
