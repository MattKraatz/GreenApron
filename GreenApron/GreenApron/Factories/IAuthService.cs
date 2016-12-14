using System.Net.Http;
using System.Threading.Tasks;

namespace GreenApron
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(User user);
        Task<AuthResponse> LoginAsync(User user);
    }
}