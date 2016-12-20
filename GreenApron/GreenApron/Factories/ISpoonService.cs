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
        Task<List<Recipe>> GetRandomRecipeAsync();
        Task<Recipe> GetRecipeByIdAsync(int recipeId);
        Task<List<Product>> GetProductByQuery(string productSearchString);
    }
}
