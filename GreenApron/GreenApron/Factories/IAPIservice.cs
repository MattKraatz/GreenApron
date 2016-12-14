using System.Net.Http;
using System.Threading.Tasks;

namespace GreenApron
{
    public interface IAPIservice
    {
        Task<JsonResponse> AddPlan(PlanRequest plan);
    }
}