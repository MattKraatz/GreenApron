using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnitsNet;

namespace WebAPI
{
    public static class Normalizer
    {
        // Convert externally-provided unit name to database preference
        // Default returns the unitName as-is
        public static string UnitName(string unitName)
        {
            string output;
            switch (unitName.ToLower())
            {
                // Ounce
                case "o":
                case "oz":
                case "ozs":
                case "ounce":
                case "ounces":
                    output = "ounce";
                    break;
                // Teaspoon
                case "t":
                case "tsp":
                case "tsps":
                case "teaspoon":
                case "teaspoons":
                    output = "tsp";
                    break;
                // Tablespoon
                case "tb":
                case "tbs":
                case "tablespoon":
                case "tablespoons":
                case "tbl":
                    output = "tbs";
                    break;
                // Cup
                case "c":
                case "cup":
                case "cups":
                    output = "cup";
                    break;
                // Pint
                case "p":
                case "pt":
                case "pts":
                case "pint":
                case "pints":
                    output = "pint";
                    break;
                // Quart
                case "qt":
                case "quart":
                case "quarts":
                    output = "quart";
                    break;
                // Gallon
                case "g":
                case "gal":
                case "gals":
                case "gallon":
                case "gallons":
                    output = "gallon";
                    break;
                // Pound
                case "lb":
                case "lbs":
                case "pound":
                case "pounds":
                    output = "pound";
                    break;
                default:
                    output = unitName.ToLower();
                    break;
            }
            return output;
        }

        // Convert externally-provided unit name to database preference
        // Default returns the unitName as-is
        public static IngredientComparitor IPantry(List<IPantryItem> items)
        {
            Volume totalVol = new Volume();
            double totalPound = 0;
            double totalCount = 0;
            string countUnit = "";

            foreach (IPantryItem item in items)
            {
                switch (item.Unit)
                {
                    case "pinch":
                        totalVol = Volume.FromUsTeaspoons(item.Amount / 16);
                        break;
                    case "dash":
                        totalVol = Volume.FromUsTeaspoons(item.Amount / 16);
                        break;
                    // Ounce
                    case "ounce":
                        totalVol += Volume.FromUsOunces(item.Amount);
                        break;
                    // Teaspoon
                    case "teaspoon":
                        totalVol += Volume.FromUsTeaspoons(item.Amount); ;
                        break;
                    // Tablespoon
                    case "tablespoon":
                        totalVol += Volume.FromUsTablespoons(item.Amount);
                        break;
                    // Cup
                    case "cup":
                        totalVol += Volume.FromUsCustomaryCups(item.Amount);
                        break;
                    // Pint
                    case "pint":
                        totalVol += Volume.FromUsGallons(item.Amount * 8);
                        break;
                    // Quart
                    case "quart":
                        totalVol += Volume.FromUsGallons(item.Amount * 4);
                        break;
                    // Gallon
                    case "gallon":
                        totalVol += Volume.FromUsGallons(item.Amount);
                        break;
                    // Pound
                    case "pound":
                        totalPound += item.Amount;
                        break;
                    default:
                        totalCount += item.Amount;
                        countUnit = item.Unit;
                        break;
                }
            }
            // If the ingredient units are a volume measurement, convert to ounces
            var output = new IngredientComparitor() { IngredientId = items.ElementAt(0).IngredientId, Count = totalCount, CountUnit = countUnit } ;
            if (totalVol.UsOunces > 0)
            {
                output.Amount = totalVol.UsOunces;
                output.Unit = "ounce";
            }
            else if (totalPound > 0)
            {
                output.Amount = totalPound;
                output.Unit = "pound";
            }
            return output;
        }
    }
}
