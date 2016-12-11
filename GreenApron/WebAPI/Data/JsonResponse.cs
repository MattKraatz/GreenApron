using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Data
{
    public class JsonResponse
    {
        public bool success { get; set; }
        public string message { get; set; }
        public User user { get; set; }
    }
}
