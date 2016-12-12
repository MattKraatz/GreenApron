using System.Net.Http;
using System.Threading.Tasks;

namespace GreenApron
{
    public interface IAuthService
    {
        Task<JsonResponse> RegisterAsync(User user);
        Task<JsonResponse> LoginAsync(User user);
    }
}