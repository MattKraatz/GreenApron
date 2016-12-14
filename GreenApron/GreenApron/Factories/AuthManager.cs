using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GreenApron
{
    public class AuthManager
    {
        IAuthService _authService;
        public User loggedInUser;

        public AuthManager (IAuthService service)
        {
            _authService = service;
        }

        public Task<AuthResponse> RegisterAsync(User user)
        {
            return _authService.RegisterAsync(user);
        }

        public Task<AuthResponse> LoginAsync(User user)
        {
            return _authService.LoginAsync(user);
        }

    }
}