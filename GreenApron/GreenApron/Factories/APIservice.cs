using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GreenApron
{
    public class APIservice : IAPIservice
    {
        private HttpClient client;

        public APIservice()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
        }

        public async Task<JsonResponse> AddPlan(PlanRequest plan)
        {
            var uri = new Uri(string.Format(Keys.WebAPI + "/plan/addplan", string.Empty));
            try
            {
                var json = JsonConvert.SerializeObject(plan);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(uri, content);
                var JSONstring = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<JsonResponse>(JSONstring);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        public async Task<BookmarkResponse> AddBookmark(BookmarkRequest bookmark)
        {
            var uri = new Uri(string.Format(Keys.WebAPI + "/bookmark/addbookmark", string.Empty));
            try
            {
                var json = JsonConvert.SerializeObject(bookmark);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(uri, content);
                var JSONstring = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<BookmarkResponse>(JSONstring);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        public async Task<BookmarkResponse> GetBookmarks()
        {
            var uri = new Uri(string.Format(Keys.WebAPI + "/bookmark/getall/" + App.AuthManager.loggedInUser.UserId.ToString(), string.Empty));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                var JSONstring = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<BookmarkResponse>(JSONstring);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        public async Task<GroceryResponse> GetGroceryItems()
        {
            var uri = new Uri(string.Format(Keys.WebAPI + "/grocery/getall/" + App.AuthManager.loggedInUser.UserId.ToString(), string.Empty));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                var JSONstring = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<GroceryResponse>(JSONstring);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        public async Task<JsonResponse> UpdateGroceryItems(GroceryRequest request)
        {
            var uri = new Uri(string.Format(Keys.WebAPI + "/grocery/update", string.Empty));
            try
            {
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(uri, content);
                var JSONstring = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<JsonResponse>(JSONstring);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        public async Task<InventoryResponse> GetInventoryItems()
        {
            var uri = new Uri(string.Format(Keys.WebAPI + "/inventory/getall/" + App.AuthManager.loggedInUser.UserId.ToString(), string.Empty));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                var JSONstring = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<InventoryResponse>(JSONstring);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        public async Task<PlanResponse> GetActivePlans()
        {
            var uri = new Uri(string.Format(Keys.WebAPI + "/plan/getall/" + App.AuthManager.loggedInUser.UserId.ToString(), string.Empty));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                var JSONstring = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<PlanResponse>(JSONstring);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        public async Task<JsonResponse> CompletePlan(Guid planId)
        {
            var uri = new Uri(string.Format(Keys.WebAPI + "/plan/complete/" + planId, string.Empty));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                var JSONstring = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<JsonResponse>(JSONstring);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        public async Task<JsonResponse> AddInventoryItem(Ingredient item)
        {
            var uri = new Uri(string.Format(Keys.WebAPI + "/inventory/add/" + App.AuthManager.loggedInUser.UserId, string.Empty));
            try
            {
                var json = JsonConvert.SerializeObject(item);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(uri, content);
                var JSONstring = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<JsonResponse>(JSONstring);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        public async Task<JsonResponse> AddGroceryItem(Ingredient item)
        {
            var uri = new Uri(string.Format(Keys.WebAPI + "/grocery/add/" + App.AuthManager.loggedInUser.UserId, string.Empty));
            try
            {
                var json = JsonConvert.SerializeObject(item);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(uri, content);
                var JSONstring = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<JsonResponse>(JSONstring);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        public async Task<JsonResponse> UpdateInventoryItems(InventoryRequest request)
        {
            var uri = new Uri(string.Format(Keys.WebAPI + "/inventory/update", string.Empty));
            try
            {
                var json = JsonConvert.SerializeObject(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(uri, content);
                var JSONstring = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<JsonResponse>(JSONstring);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        public async Task<JsonResponse> DeleteInventoryItem(Guid inventoryItemId)
        {
            var uri = new Uri(string.Format(Keys.WebAPI + "/inventory/delete/" + inventoryItemId, string.Empty));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                var JSONstring = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<JsonResponse>(JSONstring);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        public async Task<JsonResponse> DeleteGroceryItem(Guid groceryItemId)
        {
            var uri = new Uri(string.Format(Keys.WebAPI + "/grocery/delete/" + groceryItemId, string.Empty));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                var JSONstring = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<JsonResponse>(JSONstring);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        public async Task<BookmarkResponse> CheckBookmark(BookmarkRequest bookmark)
        {
            var uri = new Uri(string.Format(Keys.WebAPI + "/bookmark/check", string.Empty));
            try
            {
                var json = JsonConvert.SerializeObject(bookmark);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(uri, content);
                var JSONstring = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<BookmarkResponse>(JSONstring);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        public async Task<JsonResponse> DeleteBookmark(Guid id)
        {
            var uri = new Uri(string.Format(Keys.WebAPI + "/bookmark/delete/" + id, string.Empty));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                var JSONstring = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<JsonResponse>(JSONstring);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        public async Task<JsonResponse> DeletePlan(Guid id)
        {
            var uri = new Uri(string.Format(Keys.WebAPI + "/plan/delete/" + id, string.Empty));
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                var JSONstring = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<JsonResponse>(JSONstring);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
    }
}
