using System;

namespace GreenApron
{
    public class BookmarkRequest
    {
        public Guid userId { get; set; }
        public int RecipeId { get; set; }
        public string Title { get; set; }
        public string ImageURL { get; set; }
    }
}
