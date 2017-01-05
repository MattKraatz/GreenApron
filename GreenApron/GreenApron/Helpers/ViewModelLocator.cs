using System;
using System.Collections.Generic;

namespace GreenApron
{
	public static class ViewModelLocator
		{

			private static List<InventoryListGroup> _pantryVM;

			public static List<InventoryListGroup> pantryVM
			{
				get
				{
					if (_pantryVM == null)
					{
					_pantryVM = new List<InventoryListGroup>();
					var item = new dbIngredient() { aisle = "test aisle", ingredientName = "broccoli" };
					_pantryVM.Add(new InventoryListGroup(item.aisle, item.aisle) { new InventoryItem() { Amount = 2, AmountUnit = "2 Tsp", Unit = "Tsp", Count = 1, Empty = false, Ingredient = item, Rebuy = false } });
					}
				return _pantryVM;
				}
			}

		}
}
