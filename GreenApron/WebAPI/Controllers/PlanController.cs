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
            // Loop over recipe ingredients and add grocery items for each
            foreach (Ingredient ingredient in plan.recipe.extendedIngredients)
            {
                var newGroceryItem = new GroceryItem { IngredientName = ingredient.name, Amount = ingredient.amount, Unit = ingredient.unit, Aisle = ingredient.aisle, ImageURL = ingredient.image, PlanId = newPlan.PlanId, UserId = plan.userId };
                _context.GroceryItem.Add(newGroceryItem);
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
        public async Task<PlanResponse> GetAll([FromRoute] Guid userId)
        {
            var plans = await _context.Plan.Where(p => p.UserId == userId).ToListAsync();
            if (plans.Count < 1)
            {
                return new PlanResponse { success = false, message = "No plans were found, have you added any?" };
            }
            return new PlanResponse { success = true, message = "Bookmark(s) retrieved successfully.", plans = plans };
        }
    }
}
