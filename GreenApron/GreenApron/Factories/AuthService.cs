using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GreenApron
{
    public class AuthService : IAuthService
    {
        private HttpClient client;

        public AuthService()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
        }

        public async Task<AuthResponse> RegisterAsync(User user)
        {
            var uri = new Uri(string.Format(Keys.WebAPI + "/auth/register", string.Empty));
            try
            {
                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(uri, content);
                var JSONstring = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AuthResponse>(JSONstring);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        public async Task<AuthResponse> LoginAsync(User user)
        {
            var uri = new Uri(string.Format(Keys.WebAPI + "/auth/login", string.Empty));
            try
            {
                var json = JsonConvert.SerializeObject(user);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await client.PostAsync(uri, content);
                var JSONstring = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<AuthResponse>(JSONstring);
            }
            catch
            {
                throw new NotImplementedException();
            }
        }
    }
}
