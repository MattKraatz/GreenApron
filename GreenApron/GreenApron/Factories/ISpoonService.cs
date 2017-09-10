using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenApron
{
    public interface ISpoonService
    {
        Task<List<Recipe>> GetRandomRecipesAsync();
        Task<Recipe> GetRecipeByIdAsync(int recipeId);
        Task<List<Ingredient>> GetProductByQuery(string productSearchString);
        Task<RecipeResult> GetRecipesByQueryAsync(string query, int offset);
		Task<RecipeIngredsPreview[]> GetRecipeByIngreds(List<string> ingreds, int offset);
    }
}
