using System.Collections.Generic;

namespace WebAPI
{
    public class BookmarkResponse : JsonResponse
    {
        public List<Bookmark> bookmarks { get; set; }
    }
}
