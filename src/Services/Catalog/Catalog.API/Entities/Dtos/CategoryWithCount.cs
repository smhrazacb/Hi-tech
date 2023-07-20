namespace Catalog.API.Entities.Dtos
{
    public class CategoryWithCount
    {
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public int SubCategoryCount { get; set; }
    }
    public class CategorySubWithCountDto
    {
        public string CategoryName { get; set; }
        public IEnumerable<SubCategoryDto> SubCategorycount { get; set; }
    }
    public class SubCategoryDto
    {
        public string SubCategoryName { get; set; }
        public int SubCategoryCount { get; set; }
    }

}