using System;

namespace GreenApron
{
    public class PlanRequest
    {
        public Guid userId { get; set; }
        public DateTime date { get; set; }
        public string meal { get; set; }
        public int servingsYield { get; set; }
        public Recipe recipe { get; set; }
    }
}
