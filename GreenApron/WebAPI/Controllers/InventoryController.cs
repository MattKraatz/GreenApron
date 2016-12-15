using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    public class InventoryController : Controller
    {
        private GreenApronContext _context { get; set; }

        public InventoryController(GreenApronContext context)
        {
            _context = context;
        }

        // GET api/inventory/getall/{userId}
        // Returns all active inventory items
        [HttpGet("{userId}")]
        public async Task<JsonResult> GetAll([FromRoute] Guid userId)
        {
            var items = await _context.InventoryItem.Where(ii => ii.Amount > 0).ToListAsync();
            if (items.Count < 1)
            {
                return Json(new InventoryResponse { success = false, message = "No inventory items were found, have you added any? " });
            }
            return Json(new InventoryResponse { success = true, message = "Grocery Item(s) retrieved successfully", InventoryItems = items });
        }
    }
}
