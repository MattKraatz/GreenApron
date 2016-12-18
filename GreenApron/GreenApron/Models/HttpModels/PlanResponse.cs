using System.Collections.Generic;

namespace GreenApron
{
    public class PlanResponse : JsonResponse
    {
        public List<Plan> plans { get; set; }
    }
}
