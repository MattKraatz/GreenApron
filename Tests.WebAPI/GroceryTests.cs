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
            var user = await _task.GetUser();
            var response = await _task.AddGrocery(user.user.UserId);
            Assert.NotNull(response);
            Assert.True(response.success);
            var items = await _task.GetGrocery(user.user.UserId);
            foreach (GroceryItem item in items.GroceryItems)
            {
                await _task.DeleteGrocery(item.GroceryItemId);
            }
        }

        [Fact]
        public async void CanDeleteGroceryItem()
        {
            var user = await _task.GetUser();
            await _task.AddGrocery(user.user.UserId);
            var items = await _task.GetGrocery(user.user.UserId);
            var response = await _task.DeleteGrocery(items.GroceryItems[0].GroceryItemId);
            Assert.NotNull(response);
            Assert.True(response.success);
        }

        [Fact]
        public async void CanGetGroceryItems()
        {
            var user = await _task.GetUser();
            await _task.AddGrocery(user.user.UserId);
            var response = await _task.GetGrocery(user.user.UserId);
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
            var user = await _task.GetUser();
            await _task.AddGrocery(user.user.UserId);
            var items = await _task.GetGrocery(user.user.UserId);
            var item = items.GroceryItems[0];
            item.Amount = 50;
            var request = new GroceryRequest();
            request.items = new List<GroceryItem>();
            request.items.Add(item);
            await _task.UpdateGrocery(request);
            var itemsAgain = await _task.GetGrocery(user.user.UserId);
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
            var user = await _task.GetUser();
            await _task.AddGrocery(user.user.UserId);
            var items = await _task.GetGrocery(user.user.UserId);
            var item = items.GroceryItems[0];
            item.Deleted = true;
            var request = new GroceryRequest();
            request.items = new List<GroceryItem>();
            request.items.Add(item);
            await _task.UpdateGrocery(request);
            var itemsAgain = await _task.GetGrocery(user.user.UserId);
            Assert.NotNull(itemsAgain);
            Assert.False(itemsAgain.success);
        }

        [Fact]
        public async void CanUpdateGroceryItemPurchased()
        {
            var user = await _task.GetUser();
            await _task.AddGrocery(user.user.UserId);
            var items = await _task.GetGrocery(user.user.UserId);
            var item = items.GroceryItems[0];
            item.Purchased = true;
            var request = new GroceryRequest();
            request.items = new List<GroceryItem>();
            request.items.Add(item);
            await _task.UpdateGrocery(request);
            var itemsAgain = await _task.GetGrocery(user.user.UserId);
            Assert.NotNull(itemsAgain);
            Assert.False(itemsAgain.success);
            var pantry = await _task.GetInventory(user.user.UserId);
            foreach (InventoryItem pant in pantry.InventoryItems)
            {
                await _task.DeleteInventory(pant.InventoryItemId);
            }
        }
    }
}
