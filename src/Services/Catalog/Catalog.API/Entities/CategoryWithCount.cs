namespace Catalog.API.Entities
{
    public class CategoryWithCount
    {
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public int SubCategoryCount { get; set; }
    }
}