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

        public async Task<List<Recipe>> GetRandomRecipeAsync()
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
    }
}
