using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GreenApron
{
    public class Bookmark
    {
        public Guid BookmarkId { get; set; }
        public Guid UserId { get; set; }
        public int RecipeId { get; set; }
        public string Title { get; set; }
        public string ImageURL { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
