using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace GreenApron
{
    public class SpoonService : ISpoonService
    {
        private HttpClient client;

        public SpoonService()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("X-Mashape-Key", Keys.SpoonKey);
        }

        public async Task<List<Recipe>> GetRandomRecipesAsync()
        {
            var uri = new Uri(string.Format(Keys.SpoonURI + "/recipes/random?limitLicense=false&number=1", string.Empty));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                var JSONstring = await response.Content.ReadAsStringAsync();
                var recipeArray = JsonConvert.DeserializeObject<RecipeArray>(JSONstring);
                var recipes = new List<Recipe>();
                foreach (Recipe item in recipeArray.recipes)
                {
                    recipes.Add(item);
                }
                return recipes;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        public async Task<RecipeResult> GetRecipesByQueryAsync(string query)
        {
            var uri = new Uri(string.Format(Keys.SpoonURI + "/recipes/search?instructionsRequired=true&limitLicense=true&number=10&offset=0&query=" + query, string.Empty));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                var JSONstring = await response.Content.ReadAsStringAsync();
                var results = JsonConvert.DeserializeObject<RecipeResult>(JSONstring);
                return results;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        public async Task<Recipe> GetRecipeByIdAsync(int recipeId)
        {
            var uri = new Uri(string.Format(Keys.SpoonURI + "/recipes/" + recipeId + "/information", string.Empty));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                var JSONstring = await response.Content.ReadAsStringAsync();
                var recipe = JsonConvert.DeserializeObject<Recipe>(JSONstring);
                return recipe;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        public async Task<List<Ingredient>> GetProductByQuery(string productSearchString)
        {
            var uri = new Uri(string.Format(Keys.SpoonURI + "/food/ingredients/autocomplete?metaInformation=true&number=10&query=" + productSearchString, string.Empty));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                var test = response;
                var JSONstring = await response.Content.ReadAsStringAsync();
                var productArray = JsonConvert.DeserializeObject<Ingredient[]>(JSONstring);
                var products = new List<Ingredient>();
                foreach (Ingredient item in productArray)
                {
                    products.Add(item);
                }
                return products;
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
    }
}
