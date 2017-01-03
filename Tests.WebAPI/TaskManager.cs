﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI;

namespace Tests.WebAPI
{
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
            return await _authCtrl.Register(user);
        }

        public async Task<JsonResponse> DeleteUser()
        {
            return await _authCtrl.Delete(TestKeys.Username);
        }

        public async Task<AuthResponse> LoginUser(bool correct)
        {
            var user = new User { Username = TestKeys.Username };
            user.Password = correct ? TestKeys.Password : "wrongPass";
            return await _authCtrl.Login(user);
        }

        public async Task<AuthResponse> GetUser()
        {
            var user = new User { Username = TestKeys.Username + "1", FirstName = "Test", LastName = "User", Password = TestKeys.Password };
            var response = await _authCtrl.Login(user);
            if (response.success)
            {
                return response;
            } else
            {
                return await _authCtrl.Register(user);
            }
        }


        // BookmarkController Operations
        public async Task<BookmarkResponse> AddBookmark(Guid userId)
        {
            var mark = new BookmarkRequest { userId = userId, RecipeId = 556177, Title = "Ramen Noodle Coleslaw", ImageURL = "https://spoonacular.com/recipeImages/Ramen-Noodle-Coleslaw-556177.jpg" };
            return await _bookCtrl.AddBookmark(mark);
        }

        public async Task<BookmarkResponse> GetBookmarks(Guid userId)
        {
            return await _bookCtrl.GetAll(userId);
        }

        public async Task<JsonResponse> DeleteBookmark(Guid id)
        {
            return await _bookCtrl.Delete(id);
        }

        public async Task<BookmarkResponse> CheckBookmark(Guid userId)
        {
            var mark = new BookmarkRequest { userId = userId, RecipeId = 556177, Title = "Ramen Noodle Coleslaw", ImageURL = "https://spoonacular.com/recipeImages/Ramen-Noodle-Coleslaw-556177.jpg" };
            return await _bookCtrl.Check(mark);
        }

        // GroceryController Operations
        public async Task<GroceryResponse> GetGrocery(Guid userId)
        {
            return await _grocCtrl.GetAll(userId);
        }

        public async Task<JsonResponse> UpdateGrocery(GroceryRequest update)
        {
            return await _grocCtrl.Update(update);
        }

        public async Task<JsonResponse> AddGrocery(Guid userId)
        {
            var item = new extIngredient
            {
                id = 2053,
                aisle = "Oil, Vinegar, Salad Dressing",
                image = "https://spoonacular.com/cdn/ingredients_100x100/vinegar-(white).jpg",
                name = "vinegar",
                amount = 3,
                unit = "tablespoons"
            };
            return await _grocCtrl.Add(item, userId);
        }

        public async Task<JsonResponse> DeleteGrocery(Guid id)
        {
            return await _grocCtrl.Delete(id);
        }

        // InventoryController Operations
        public async Task<InventoryResponse> GetInventory(Guid userId)
        {
            return await _invCtrl.GetAll(userId);
        }

        public async Task<JsonResponse> UpdateInventory(InventoryRequest update)
        {
            return await _invCtrl.Update(update);
        }

        public async Task<JsonResponse> AddInventory(Guid userId)
        {
            var item = new extIngredient
            {
                id = 2053,
                aisle = "Oil, Vinegar, Salad Dressing",
                image = "https://spoonacular.com/cdn/ingredients_100x100/vinegar-(white).jpg",
                name = "vinegar",
                amount = 3,
                unit = "tablespoons"
            };
            return await _invCtrl.Add(item, userId);
        }

        public async Task<JsonResponse> DeleteInventory(Guid id)
        {
            return await _invCtrl.Delete(id);
        }

    }
}