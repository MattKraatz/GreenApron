using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI
{
    [Route("api/[controller]")]
    public class PlanController : Controller
    {
        private GreenApronContext _context { get; set; }
        private IngredientManager _ingManager { get; set; }

        public PlanController(GreenApronContext context)
        {
            _context = context;
            _ingManager = new IngredientManager(_context);
        }
        
        // POST api/plan
        [HttpPost]
        public async Task<JsonResponse> Post([FromBody] PlanRequest plan)
        {
            // Check ModelState
            if (!ModelState.IsValid)
            {
                // If invalid, return error message
                return new JsonResponse { success = false, message = "Something went wrong, please resubmit with all required fields." };
            }
            // Create Plan record and save to database
            var newPlan = new Plan { Date = plan.date, Meal = plan.meal, ServingsYield = plan.servingsYield, RecipeName = plan.recipe.title, RecipeId = plan.recipe.id, RecipeImage = plan.recipe.image, UserId = plan.userId};
            _context.Plan.Add(newPlan);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch
            {
                return new JsonResponse { success = false, message = "Something went wrong while saving your plan to the database, please try again." };
            }

            // Retrieve all active plans to be used in the following loop
            var activePlans = await _context.Plan.Where(p => DateTime.Compare(p.Date, DateTime.Now) >= 0).Where(p=> p.DateCompleted == null).Select(p => p.PlanId).ToListAsync();
            // Loop over recipe ingredients and add grocery items for each
            foreach (extIngredient planIngredient in plan.recipe.extendedIngredients)
            {
                // Add this ingredient to the database, if it doesn't already exist
                Ingredient ingredient = await _ingManager.CheckDB(planIngredient);
                // Handle database error
                if (ingredient.IngredientId == -1)
                {
                    return new JsonResponse { success = false, message = "Something went wrong while saving your ingredients to the database, please try again." };
                }
                // Normalize the ingredient unit name
                planIngredient.unit = Normalizer.UnitName(planIngredient.unit);
                // Add a PlanIngredient record for this ingredient
                var newPlanIngredient = new PlanIngredient { PlanId = newPlan.PlanId, Amount = planIngredient.amount, Unit = planIngredient.unit, IngredientId = ingredient.IngredientId };
                _context.PlanIngredient.Add(newPlanIngredient);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    return new JsonResponse { success = false, message = "Something went wrong while saving your plan ingredients to the database, please try again." };
                }
                // Sum up total requirement for this ingredient for all active plans
                List<IPantryItem> totalRequirement = await _context.PlanIngredient.Where(pi => _context.Plan.Where(p => DateTime.Compare(p.Date, DateTime.Now) >= 0).Where(p => p.DateCompleted == null).SingleOrDefault(p => p.PlanId == pi.PlanId) != null).Where(pi => pi.IngredientId == planIngredient.id).ToListAsync<IPantryItem>();
                // Normalize total ingredient requirement
                IngredientComparitor req = Normalizer.IPantry(totalRequirement);

                // Sum up total existing inventory for this ingredient
                List<IPantryItem> totalInventory = await _context.InventoryItem.Where(ii => ii.IngredientId == planIngredient.id).ToListAsync<IPantryItem>();
                // Normalize total ingredient requirement
                IngredientComparitor inv = new IngredientComparitor();
                if (totalInventory.Count > 0)
                {
                    inv = Normalizer.IPantry(totalInventory);
                } else
                {
                    inv.Amount = 0;
                    inv.Count = 0;
                }

                // Check whether totalInventory and totalRequirement are the same type first
                if (inv.Amount < req.Amount || inv.Count < req.Count)
                {
                    // Find any existing GroceryItem record for this ingredient, if it exists, modify it with the new quantity
                    List<GroceryItem> groceryItems = await _context.GroceryItem.Where(gi => gi.DateCompleted == null).Where(gi => gi.IngredientId == planIngredient.id).ToListAsync<GroceryItem>();
                    if (groceryItems.Count > 0)
                    {
                        foreach (GroceryItem item in groceryItems)
                        {
                            switch (item.Unit)
                            {
                                case "pinch": case "dash": case "ounce": case "cup": case "pint": case "quart": case "gallon": case "teaspoon": case "tablespoon":
                                    var display = DisplayUnit.FromOunces(req.Amount);
                                    item.Amount = display.Amount;
                                    item.Unit = display.Unit;
                                    req.Amount = 0;
                                    break;
                                case "pound":
                                    item.Amount = req.Amount;
                                    req.Amount = 0;
                                    break;
                                default:
                                    item.Amount = req.Count;
                                    req.Count = 0;
                                    break;
                            }
                            _context.Entry(item).State = EntityState.Modified;
                        }
                    }
                    // Else add a new GroceryItem record
                    if (req.Amount > 0 && req.Unit != "pound")
                    {
                        var display = DisplayUnit.FromOunces(req.Amount);
                        var newGroceryItem = new GroceryItem { IngredientId = req.IngredientId, Amount = display.Amount, Unit = display.Unit, UserId = plan.userId };
                        _context.GroceryItem.Add(newGroceryItem);
                    } else if (req.Amount > 0 && req.Unit == "pound")
                    {
                        var newGroceryItem = new GroceryItem { IngredientId = req.IngredientId, Amount = req.Amount, Unit = req.Unit, UserId = plan.userId };
                        _context.GroceryItem.Add(newGroceryItem);
                    }
                    if (req.Count > 0)
                    {
                        var newGroceryItem = new GroceryItem { IngredientId = req.IngredientId, Amount = req.Count, Unit = req.CountUnit, UserId = plan.userId };
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
                return new JsonResponse { success = false, message = "Something went wrong while saving your grocery items to the database, please try again." };
            }

            return new JsonResponse { success = true, message = "Plan saved successfully"};
        }

        // GET api/plan/{userId}
        // Returns all active plans
        [HttpGet("{userId}")]
        public async Task<PlanResponse> Get([FromRoute] Guid userId)
        {
            var plans = await _context.Plan.Where(p => DateTime.Compare(p.Date, DateTime.Now) >= 0)
                .Where(p => p.UserId == userId).Where(p => p.DateCompleted == null)
                .Include(p => p.PlanIngredients).ToListAsync();
            if (plans.Count < 1)
            {
                return new PlanResponse { success = false, message = "No plans were found, have you added any?" };
            }
            return new PlanResponse { success = true, message = "Plan(s) retrieved successfully.", plans = plans };
        }

        // PUT api/plan/{planId}
        // Completes a plan, deducts amounts from inventory items
        // Refactored to REST so it will accept any kind of updates, currently only completing the plan however
        [HttpPut]
        public async Task<JsonResponse> Put([FromBody] Plan planRequest)
        {
            var plan = await _context.Plan.SingleOrDefaultAsync(p => p.PlanId == planRequest.PlanId);
            if (plan == null)
            {
                return new JsonResponse { success = false, message = "Something went wrong, please refresh your client." };
            }
            if (planRequest.DateCompleted != null)
            {
                var planIngredients = await _context.PlanIngredient.Where(pi => pi.PlanId == plan.PlanId).ToListAsync();
                if (planIngredients.Count > 0)
                {
                    foreach (PlanIngredient ingredient in planIngredients)
                    {
                        _context.PlanIngredient.Remove(ingredient);
                        var inventoryItem = await _context.InventoryItem.Where(ii => ii.IngredientId == ingredient.IngredientId).FirstOrDefaultAsync(ii => ii.Unit == ingredient.Unit);
                        if (inventoryItem != null)
                        {
                            if (inventoryItem.Amount - ingredient.Amount <= 0)
                            {
                                _context.InventoryItem.Remove(inventoryItem);
                            }
                            else
                            {
                                inventoryItem.Amount -= ingredient.Amount;
                                _context.Entry(inventoryItem).State = EntityState.Modified;
                            }
                        }
                    }
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch
                    {
                        return new JsonResponse { success = false, message = "Something went wrong while saving to the database, please try again." };
                    }
                }
                plan.DateCompleted = planRequest.DateCompleted;
                _context.Entry(plan).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    return new JsonResponse { success = false, message = "Something went wrong while saving to the database, please try again." };
                }
                return new JsonResponse { success = true, message = "Plan updated successfully" };
            } else
            {
                return new JsonResponse { success = false, message = "Nothing to update." };
            }
        }

        // DELETE api/plan
        [HttpDelete("{id}")]
        public async Task<JsonResponse> Delete([FromRoute] Guid id)
        {
            // Find the plan record in the database
            var dbItem = await _context.Plan.SingleOrDefaultAsync(p => p.PlanId == id);
            if (dbItem != null)
            {
                var planIngreds = await _context.PlanIngredient.Where(pi => pi.PlanId == id).ToListAsync();
                foreach (PlanIngredient ingred in planIngreds)
                {
                    _context.PlanIngredient.Remove(ingred);
                }
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    return new JsonResponse { success = false, message = "Something went wrong while saving to the database, please try again." };
                }
                _context.Plan.Remove(dbItem);
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
