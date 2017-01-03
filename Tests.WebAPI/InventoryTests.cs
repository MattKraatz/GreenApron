using System.Collections.Generic;
using WebAPI;
using Xunit;

namespace Tests.WebAPI
{
    public class InventoryTests
    {
        private TaskManager _task { get; set; } = new TaskManager();

        public InventoryTests()
        {
        }

        [Fact]
        public async void CanAddInventoryItem()
        {
            var user = await _task.GetUser();
            var response = await _task.AddInventory(user.user.UserId);
            Assert.NotNull(response);
            Assert.True(response.success);
            var items = await _task.GetInventory(user.user.UserId);
            foreach (InventoryItem item in items.InventoryItems)
            {
                await _task.DeleteInventory(item.InventoryItemId);
            }
        }

        [Fact]
        public async void CanDeleteInventoryItems()
        {
            var user = await _task.GetUser();
            await _task.AddInventory(user.user.UserId);
            var items = await _task.GetInventory(user.user.UserId);
            var response = await _task.DeleteInventory(items.InventoryItems[0].InventoryItemId);
            Assert.NotNull(response);
            Assert.True(response.success);
        }

        [Fact]
        public async void CanGetInventoryItems()
        {
            var user = await _task.GetUser();
            await _task.AddInventory(user.user.UserId);
            var response = await _task.GetInventory(user.user.UserId);
            Assert.NotNull(response);
            Assert.True(response.success);
            Assert.True(response.InventoryItems.Count > 0);
            foreach (InventoryItem item in response.InventoryItems)
            {
                await _task.DeleteInventory(item.InventoryItemId);
            }
        }

        [Fact]
        public async void CanUpdateInventoryItemAmount()
        {
            var user = await _task.GetUser();
            await _task.AddInventory(user.user.UserId);
            var items = await _task.GetInventory(user.user.UserId);
            var item = items.InventoryItems[0];
            item.Amount = 50;
            var request = new InventoryRequest();
            request.items = new List<InventoryItem>();
            request.items.Add(item);
            await _task.UpdateInventory(request);
            var itemsAgain = await _task.GetInventory(user.user.UserId);
            var itemAgain = itemsAgain.InventoryItems[0];
            Assert.NotNull(itemsAgain);
            Assert.True(itemAgain.Amount == item.Amount);
            foreach (InventoryItem inv in itemsAgain.InventoryItems)
            {
                await _task.DeleteInventory(inv.InventoryItemId);
            }
        }

        [Fact]
        public async void CanUpdateInventoryItemEmptied()
        {
            var user = await _task.GetUser();
            await _task.AddInventory(user.user.UserId);
            var items = await _task.GetInventory(user.user.UserId);
            var item = items.InventoryItems[0];
            item.Empty = true;
            var request = new InventoryRequest();
            request.items = new List<InventoryItem>();
            request.items.Add(item);
            await _task.UpdateInventory(request);
            var itemsAgain = await _task.GetInventory(user.user.UserId);
            Assert.NotNull(itemsAgain);
            Assert.False(itemsAgain.success);
        }

        [Fact]
        public async void CanUpdateInventoryItemRebuy()
        {
            var user = await _task.GetUser();
            await _task.AddInventory(user.user.UserId);
            var items = await _task.GetInventory(user.user.UserId);
            var item = items.InventoryItems[0];
            item.Rebuy = true;
            var request = new InventoryRequest();
            request.items = new List<InventoryItem>();
            request.items.Add(item);
            await _task.UpdateInventory(request);
            var itemsAgain = await _task.GetInventory(user.user.UserId);
            Assert.NotNull(itemsAgain);
            Assert.True(itemsAgain.success);
            var grocery = await _task.GetGrocery(user.user.UserId);
            Assert.NotNull(grocery);
            Assert.True(grocery.GroceryItems.Count > 0);
            foreach (GroceryItem groc in grocery.GroceryItems)
            {
                await _task.DeleteGrocery(groc.GroceryItemId);
            }
            await _task.DeleteInventory(item.InventoryItemId);
        }
    }
}
