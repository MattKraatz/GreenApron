using System.Collections.Generic;

namespace GreenApron
{
    public class BookmarkResponse : JsonResponse
    {
        public List<Bookmark> bookmarks { get; set; }
    }
}
