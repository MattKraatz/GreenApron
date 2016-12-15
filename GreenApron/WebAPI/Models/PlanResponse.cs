using System.Collections.Generic;

namespace WebAPI
{
    public class PlanResponse : JsonResponse
    {
        public List<Plan> plans { get; set; }
    }
}
