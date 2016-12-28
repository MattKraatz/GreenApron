using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI
{
    public class IngredientManager
    {
        private GreenApronContext _context { get; set; }
        Random rand = new Random();

        public IngredientManager(GreenApronContext context)
        {
            _context = context;
        }

        // Checks the database for an existing ingredient captured from Spoonacular
        public async Task<Ingredient> CheckDB(extIngredient ingr)
        {
            Ingredient newIngredient = new Ingredient();
            var dbIngredient = await _context.Ingredient.SingleOrDefaultAsync(i => i.IngredientId == ingr.id || i.IngredientName == ingr.name);
            if (dbIngredient == null)
            {
                if (ingr.id < 1)
                {
                    newIngredient.IngredientId = rand.Next(100000000, 999999999);
                }
                else
                {
                    newIngredient.IngredientId = ingr.id;
                }
                newIngredient.IngredientName = ingr.name;
                newIngredient.Aisle = ingr.aisle;
                newIngredient.ImageURL = ingr.image;
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
                    // Return an IngredientId of -1 if there were any errors with the database
                    newIngredient.IngredientId = -1;
                    return newIngredient;
                }
                finally
                {
                    _context.Database.CloseConnection();
                }
                return newIngredient;
            };
            return dbIngredient;
        }
    }
}
