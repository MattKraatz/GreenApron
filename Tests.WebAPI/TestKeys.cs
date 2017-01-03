using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI;

namespace Tests.WebAPI
{
    public static class TestKeys
    {
        public static string Username { get; } = "TestUser192837";
        public static string Password { get; } = "Password1";
        public static List<extIngredient> Noodle { get; } = new List<extIngredient>() { new extIngredient()
        {
          id = 6016,
          aisle = "Canned and Jarred",
          image = "https =//spoonacular.com/cdn/ingredients_100x100/cream-of-chicken-soup.jpg",
          name = "cream of chicken soup",
          amount = 1,
          unit = "package",
          unitShort = "pkg",
          unitLong = "package",
          originalString = "1 package chicken flavor ramen noodle soup"
        }};
        public static Recipe RamenNoodle { get; } = new Recipe
        {
            vegetarian = false,
            vegan = false,
            glutenFree = true,
            dairyFree = true,
            veryHealthy = false,
            cheap = false,
            veryPopular = false,
            sustainable = false,
            weightWatcherSmartPoints = 4,
            gaps = "no",
            lowFodmap = false,
            ketogenic = false,
            whole30 = false,
            servings = 8,
            preparationMinutes = 5,
            cookingMinutes = 10,
            sourceUrl = "http =//www.dizzybusyandhungry.com/ramen-noodle-coleslaw/",
            spoonacularSourceUrl = "https =//spoonacular.com/ramen-noodle-coleslaw-556177",
            aggregateLikes = 221,
            creditText = "Dizzy Busy and Hungry",
            sourceName = "Dizzy Busy and Hungry",
            extendedIngredients = Noodle,
            id = 556177,
            title = "Ramen Noodle Coleslaw",
            readyInMinutes = 15,
            image = "https =//spoonacular.com/recipeImages/Ramen-Noodle-Coleslaw-556177.jpg",
            imageType = "jpg",
            instructions = "Toast the sesame seeds, about 350 degrees in the oven for about 10-15 minutes. Keep an eye on them to make sure they don't burn.Mix together the following to make the dressing: olive oil, vinegar, sugar, salt, pepper, green onions, chicken flavor packet from the ramen noodle package.Crush the ramen noodles until there are no large chunks (small chunks are OK).Combine the shredded cabbage and ramen noodles in a large bowl.Pour the dressing on the cabbage/noodle mixture and toss to coat.Top with the toasted sesame seeds and almonds."
        };
    }
}
