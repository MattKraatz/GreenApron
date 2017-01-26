namespace GreenApron
{
	public class RecipeIngredsPreview
    {
        public int id { get; set; }
        public string title { get; set; }
        public int usedIngredientCount { get; set; }
		public int missedIngredientCount { get; set; }
        public string image { get; set; }
    }
}