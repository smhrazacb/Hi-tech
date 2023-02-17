namespace Catalog.API.Entities
{
    public class CategoryWithCount
    {
        public string SubCategory { get; set; }
        public string Category { get; set; }
        public int Count { get; set; }
    }
}