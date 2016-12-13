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
            return _spoonService.GetRandomRecipeAsync();
        }
    }
}
