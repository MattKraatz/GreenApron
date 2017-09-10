namespace GreenApron
{
	public class RecipeResult
    {
        public RecipePreview[] results { get; set; }
        public string baseUri { get; set; }
        public int offset { get; set; }
        public int number { get; set; }
        public int totalResults { get; set; }
        public int processingTimeMs { get; set; }
        public long expires { get; set; }
        public bool isStale { get; set; }
    }
}
