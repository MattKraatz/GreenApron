using Microsoft.AspNetCore.Mvc;

namespace WebAPI
{
    [Route("api/v1")]
    public class Root : Controller
    {
        [HttpGet]
        public IndexResponse Index()
        {
            return new IndexResponse();
        }
    }
}
