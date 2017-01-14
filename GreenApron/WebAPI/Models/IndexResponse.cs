using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI
{
    public class IndexResponse
    {
        public struct method
        {
            public string verb;
            public string[] parameters;
            public string description;
        }

        public Dictionary<string, method[]> Directories { get; } = new Dictionary<string, method[]>
        {
            { "auth", new method[]
                {
                    new method() { verb = "GET", parameters = new string[] { "Route: string UserId","string Password" }, description = "Logs in user, returns User object with UserId" },
                    new method() { verb = "POST", parameters = new string[] { "Body: User user" }, description = "Registers user, returns User object with UserId"  }
                }
            },
            { "bookmark", new method[]
                {
                    new method() { verb = "GET", parameters = new string[] { "Route: Guid UserId" }, description = "Gets all bookmarks for a UserId" },
                    new method() { verb = "GET", parameters = new string[] { "Route: int RecipeId, Guid UserId" }, description = "Gets a bookmark matching the provided RecipeId and UserId" },
                    new method() { verb = "POST", parameters = new string[] { "Body: Bookmark bookmark" }, description = "Posts a new bookmark"  },
                    new method() { verb = "DELETE", parameters = new string[] { "Route: Guid BookmarkId" }, description = "Deletes a bookmark"  }
                }
            },
            { "grocery", new method[]
                {
                    new method() { verb = "GET", parameters = new string[] { "Route: Guid UserId" }, description = "Gets all grocery items for a UserId" },
                    new method() { verb = "POST", parameters = new string[] { "Route: Guid UserId, Body: Ingredient item" }, description = "Posts a new grocery item"  },
                    new method() { verb = "PUT", parameters = new string[] { "Body: GroceryItem[] items" }, description = "Updates a list of grocery items"  },
                    new method() { verb = "DELETE", parameters = new string[] { "Route: Guid GroceryItemId" }, description = "Deletes a grocery item"  }
                }
            },
            { "inventory", new method[]
                {
                    new method() { verb = "GET", parameters = new string[] { "Route: Guid UserId" }, description = "Gets all inventory items for a UserId" },
                    new method() { verb = "POST", parameters = new string[] { "Route: Guid UserId, Body: Ingredient item" }, description = "Posts a new inventory item"  },
                    new method() { verb = "PUT", parameters = new string[] { "Body: InventoryItem[] items" }, description = "Updates a list of inventory items"  },
                    new method() { verb = "DELETE", parameters = new string[] { "Route: Guid InventoryItemId" }, description = "Deletes an inventory item"  }
                }
            },
            { "plan", new method[]
                {
                    new method() { verb = "GET", parameters = new string[] { "Route: Guid UserId" }, description = "Gets all active plans for a UserId" },
                    new method() { verb = "POST", parameters = new string[] { "Body: Plan plan" }, description = "Posts a new plan"  },
                    new method() { verb = "PUT", parameters = new string[] { "Body: Plan plan" }, description = "Updates a plan"  },
                    new method() { verb = "DELETE", parameters = new string[] { "Route: Guid PlanId" }, description = "Deletes a plan"  }
                }
            }
        };
    }
}
