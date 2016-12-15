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

        public async Task<JsonResponse> AddBookmark(BookmarkRequest bookmark)
        {
            var uri = new Uri(string.Format(Keys.WebAPI + "/bookmark/addbookmark", string.Empty));
            try
            {
                var json = JsonConvert.SerializeObject(bookmark);
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
    }
}
