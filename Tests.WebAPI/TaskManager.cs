using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using WebAPI;

namespace Tests.WebAPI
{

    /**
     * Purpose: Generates a context and instantiates each WebAPI controller with it
     *          Provides abstractions for all controller tasks required for WebAPI testing
     */

    public class TaskManager
    {
        private GreenApronContext _context { get; set; }
        private AuthController _authCtrl { get; set; }
        private BookmarkController _bookCtrl { get; set; }
        private GroceryController _grocCtrl { get; set; }
        private InventoryController _invCtrl { get; set; }
        private PlanController _planCtrl { get; set; }

        public TaskManager()
        {
            // Provide context via dependency injection
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkSqlServer()
                .BuildServiceProvider();
            var builder = new DbContextOptionsBuilder<GreenApronContext>();
            builder.UseSqlServer(Environment.GetEnvironmentVariable("GreenApronDB"))
                    .UseInternalServiceProvider(serviceProvider);
            _context = new GreenApronContext(builder.Options);

            // Instantiate WebAPI controllers with database context
            _authCtrl = new AuthController(_context);
            _bookCtrl = new BookmarkController(_context);
            _grocCtrl = new GroceryController(_context);
            _invCtrl = new InventoryController(_context);
            _planCtrl = new PlanController(_context);
        }

        // AuthController Operations
        public async Task<AuthResponse> RegisterUser()
        {
            var user = new User { Username = TestKeys.Username, FirstName = "Test", LastName = "User", Password = TestKeys.Password };
            return await _authCtrl.Post(user);
        }

        public async Task<JsonResponse> DeleteUser()
        {
            return await _authCtrl.Delete(TestKeys.Username);
        }

        public async Task<AuthResponse> LoginUser(bool correct)
        {
            var user = new User { Username = TestKeys.Username };
            user.Password = correct ? TestKeys.Password : "wrongPass";
            return await _authCtrl.Get(user.Username, user.Password);
        }

        public async Task<AuthResponse> GetUser()
        {
            var user = new User { Username = TestKeys.Username + "1", FirstName = "Test", LastName = "User", Password = TestKeys.Password };
            var response = await _authCtrl.Get(TestKeys.Username + "1", TestKeys.Password);
            if (response.success)
            {
                return response;
            } else
            {
                return await _authCtrl.Post(user);
            }
        }


        // BookmarkController Operations
        public async Task<BookmarkResponse> AddBookmark()
        {
            var user = await GetUser();
            var mark = new BookmarkRequest { userId = user.user.UserId, RecipeId = 556177, Title = "Ramen Noodle Coleslaw", ImageURL = "https://spoonacular.com/recipeImages/Ramen-Noodle-Coleslaw-556177.jpg" };
            return await _bookCtrl.Post(mark);
        }

        public async Task<BookmarkResponse> GetBookmarks()
        {
            var user = await GetUser();
            return await _bookCtrl.Get(user.user.UserId);
        }

        public async Task<JsonResponse> DeleteBookmark(Guid id)
        {
            return await _bookCtrl.Delete(id);
        }

        public async Task<BookmarkResponse> CheckBookmark()
        {
            var user = await GetUser();
            return await _bookCtrl.Get(556177, user.user.UserId);
        }

        // GroceryController Operations
        public async Task<GroceryResponse> GetGrocery()
        {
            var user = await GetUser();
            return await _grocCtrl.Get(user.user.UserId);
        }

        public async Task<JsonResponse> UpdateGrocery(GroceryRequest update)
        {
            return await _grocCtrl.Put(update);
        }

        public async Task<JsonResponse> AddGrocery()
        {
            var user = await GetUser();
            var item = new extIngredient
            {
                id = 2053,
                aisle = "Oil, Vinegar, Salad Dressing",
                image = "https://spoonacular.com/cdn/ingredients_100x100/vinegar-(white).jpg",
                name = "vinegar",
                amount = 3,
                unit = "tablespoons"
            };
            return await _grocCtrl.Post(item, user.user.UserId);
        }

        public async Task<JsonResponse> DeleteGrocery(Guid id)
        {
            return await _grocCtrl.Delete(id);
        }

        // InventoryController Operations
        public async Task<InventoryResponse> GetInventory()
        {
            var user = await GetUser();
            return await _invCtrl.Get(user.user.UserId);
        }

        public async Task<JsonResponse> UpdateInventory(InventoryRequest update)
        {
            return await _invCtrl.Put(update);
        }

        public async Task<JsonResponse> AddInventory()
        {
            var user = await GetUser();
            var item = new extIngredient
            {
                id = 2053,
                aisle = "Oil, Vinegar, Salad Dressing",
                image = "https://spoonacular.com/cdn/ingredients_100x100/vinegar-(white).jpg",
                name = "vinegar",
                amount = 3,
                unit = "tablespoons"
            };
            return await _invCtrl.Post(item, user.user.UserId);
        }

        public async Task<JsonResponse> DeleteInventory(Guid id)
        {
            return await _invCtrl.Delete(id);
        }

        // PlanController Operations
        public async Task<JsonResponse> AddPlan()
        {
            var user = await GetUser();
            var request = new PlanRequest
            {
                userId = user.user.UserId, date = DateTime.Today.AddDays(1),
                meal = "Breakfast", recipe = TestKeys.RamenNoodle, servingsYield = 4
            };
            return await _planCtrl.Post(request);
        }

        public async Task<PlanResponse> GetPlans()
        {
            var user = await GetUser();
            return await _planCtrl.Get(user.user.UserId);
        }

        public async Task<JsonResponse> UpdatePlan(Plan plan)
        {
            return await _planCtrl.Put(plan);
        }

        public async Task<JsonResponse> DeletePlan(Guid id)
        {
            return await _planCtrl.Delete(id);
        }
    }
}
