﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
                var dbIngredient = await _context.Ingredient.SingleOrDefaultAsync(i => i.IngredientId == planIngredient.id || i.IngredientName == planIngredient.name);
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
                        return Json(new JsonResponse { success = false, message = "Something went wrong while saving your ingredients to the database, please try again." });
                    }
                    finally
                    {
                        _context.Database.CloseConnection();
                    }
                };
                // Normalize the ingredient unit name
                planIngredient.unit = Normalizer.UnitName(planIngredient.unit);
                // Add a PlanIngredient record for this ingredient
                var newPlanIngredient = new PlanIngredient { PlanId = newPlan.PlanId, Amount = planIngredient.amount, Unit = planIngredient.unit };
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
                    return Json(new JsonResponse { success = false, message = "Something went wrong while saving your plan ingredients to the database, please try again." });
                }
                // Sum up total requirement for this ingredient for all active plans
                // Currently assumes all ingredients share the same unit of measurement
                List<IPantryItem> totalRequirement = await _context.PlanIngredient.Where(pi => _context.Plan.Where(p => DateTime.Compare(p.Date, DateTime.Now) >= 0).SingleOrDefault(p => p.PlanId == pi.PlanId) != null).Where(pi => pi.IngredientId == planIngredient.id).ToListAsync<IPantryItem>();
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
                    List<GroceryItem> groceryItems = await _context.GroceryItem.Where(gi => gi.IngredientId == planIngredient.id).ToListAsync<GroceryItem>();
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
                    if (req.Amount > 0)
                    {
                        var display = DisplayUnit.FromOunces(req.Amount);
                        var newGroceryItem = new GroceryItem { IngredientId = req.IngredientId, Amount = display.Amount, Unit = display.Unit, UserId = plan.userId };
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
                return Json(new JsonResponse { success = false, message = "Something went wrong while saving your grocery items to the database, please try again." });
            }

            return Json( new JsonResponse { success = true, message = "Plan saved successfully"});
        }

        // GET api/plan/getall/{userId}
        // Returns all active plans
        [HttpGet("{userId}")]
        public async Task<JsonResult> GetAll([FromRoute] Guid userId)
        {
            var plans = await _context.Plan.Where(p => p.UserId == userId).Include(p => p.PlanIngredients).ToListAsync();
            if (plans.Count < 1)
            {
                return Json(new PlanResponse { success = false, message = "No plans were found, have you added any?" });
            }
            return Json(new PlanResponse { success = true, message = "Plan(s) retrieved successfully.", plans = plans });
        }

        // POST api/plan/complete/{planId}
        // Completes a plan, deducts amounts from inventory items
        [HttpGet("{planId}")]
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
