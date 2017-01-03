using System.Collections.Generic;
using WebAPI;
using Xunit;

namespace Tests.WebAPI
{
    public class GroceryTests
    {
        private TaskManager _task { get; set; } = new TaskManager();

        public GroceryTests()
        {
        }

        [Fact]
        public async void CanAddGroceryItem()
        {
            var response = await _task.AddGrocery();
            Assert.NotNull(response);
            Assert.True(response.success);
            var items = await _task.GetGrocery();
            foreach (GroceryItem item in items.GroceryItems)
            {
                await _task.DeleteGrocery(item.GroceryItemId);
            }
        }

        [Fact]
        public async void CanDeleteGroceryItem()
        {
            await _task.AddGrocery();
            var items = await _task.GetGrocery();
            var response = await _task.DeleteGrocery(items.GroceryItems[0].GroceryItemId);
            Assert.NotNull(response);
            Assert.True(response.success);
        }

        [Fact]
        public async void CanGetGroceryItems()
        {
            await _task.AddGrocery();
            var response = await _task.GetGrocery();
            Assert.NotNull(response);
            Assert.True(response.success);
            Assert.True(response.GroceryItems.Count == 1);
            foreach (GroceryItem item in response.GroceryItems)
            {
                await _task.DeleteGrocery(item.GroceryItemId);
            }
        }

        [Fact]
        public async void CanUpdateGroceryItemAmount()
        {
            await _task.AddGrocery();
            var items = await _task.GetGrocery();
            var item = items.GroceryItems[0];
            item.Amount = 50;
            var request = new GroceryRequest();
            request.items = new List<GroceryItem>();
            request.items.Add(item);
            await _task.UpdateGrocery(request);
            var itemsAgain = await _task.GetGrocery();
            Assert.NotNull(itemsAgain);
            Assert.True(itemsAgain.GroceryItems[0].Amount == item.Amount);
            foreach (GroceryItem groc in itemsAgain.GroceryItems)
            {
                await _task.DeleteGrocery(groc.GroceryItemId);
            }
        }

        [Fact]
        public async void CanUpdateGroceryItemDeleted()
        {
            await _task.AddGrocery();
            var items = await _task.GetGrocery();
            var item = items.GroceryItems[0];
            item.Deleted = true;
            var request = new GroceryRequest();
            request.items = new List<GroceryItem>();
            request.items.Add(item);
            await _task.UpdateGrocery(request);
            var itemsAgain = await _task.GetGrocery();
            Assert.NotNull(itemsAgain);
            Assert.False(itemsAgain.success);
        }

        [Fact]
        public async void CanUpdateGroceryItemPurchased()
        {
            await _task.AddGrocery();
            var items = await _task.GetGrocery();
            var item = items.GroceryItems[0];
            item.Purchased = true;
            var request = new GroceryRequest();
            request.items = new List<GroceryItem>();
            request.items.Add(item);
            await _task.UpdateGrocery(request);
            var itemsAgain = await _task.GetGrocery();
            Assert.NotNull(itemsAgain);
            Assert.False(itemsAgain.success);
            var pantry = await _task.GetInventory();
            foreach (InventoryItem pant in pantry.InventoryItems)
            {
                await _task.DeleteInventory(pant.InventoryItemId);
            }
        }
    }
}
