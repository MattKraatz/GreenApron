using System.Net.Http;
using System.Threading.Tasks;

namespace GreenApron
{
    public interface IAPIservice
    {
        Task<JsonResponse> AddPlan(PlanRequest plan);
        Task<JsonResponse> AddBookmark(BookmarkRequest bookmark);
        Task<BookmarkResponse> GetBookmarks();
        Task<GroceryResponse> GetGroceryItems();
        Task<JsonResponse> UpdateGroceryItems(GroceryRequest request);
    }
}