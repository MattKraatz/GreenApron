using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI
{
    public class BookmarkRequest
    {
        public Guid userId { get; set; }
        public int RecipeId { get; set; }
        public string Title { get; set; }
        public string ImageURL { get; set; }
    }
}
