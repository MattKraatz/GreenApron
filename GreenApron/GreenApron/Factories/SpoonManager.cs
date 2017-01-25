using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenApron
{
    public class SpoonManager
    {
        ISpoonService _spoonService;
        
        public SpoonManager(ISpoonService service)
        {
            _spoonService = service;
        }

        public Task<List<Recipe>> GetRandomRecipeAsync()
        {
            return _spoonService.GetRandomRecipesAsync();
        }

        public Task<Recipe> GetRecipeByIdAsync(int recipeId)
        {
            return _spoonService.GetRecipeByIdAsync(recipeId);
        }

        public Task<List<Ingredient>> GetProductByQuery(string productSearchString)
        {
            return _spoonService.GetProductByQuery(productSearchString);
        }

        public Task<RecipeResult> GetRecipesByQueryAsync(string query, int offset)
        {
            return _spoonService.GetRecipesByQueryAsync(query, offset);
        }
    }
}
