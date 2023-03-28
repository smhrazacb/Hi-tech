namespace Catalog.API.Entities.Dtos
{
    public class CSVDto
    {
        public List<Category> UpdateCategories { get; set; }
        public List<Category> NewCategories { get; set; }
        public List<string> InvalidEntries { get; set; }
        public List<string> DuplicatePartNumbers { get; set; }
    }
}
