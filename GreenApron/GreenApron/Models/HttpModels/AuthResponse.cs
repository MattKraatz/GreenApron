using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GreenApron
{
    public class AuthResponse : JsonResponse
    {
        public User user { get; set; }
    }
}
