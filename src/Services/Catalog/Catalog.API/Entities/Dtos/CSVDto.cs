namespace Catalog.API.Entities.Dtos
{
    public class CSVDto
    {
        public int UpdateCategoriesCount { get;  set; }
        public int NewCategoriesCount { get;  set; }
        public int InvalidEntriesCount { get;  set; }
        public int DuplicatePartNumbersCount { get;  set; }
        public List<Category> UpdateCategories { get; set; }
        public List<Category> NewCategories { get; set; }
        public List<string> InvalidEntries { get; set; }
        public List<string> DuplicatePartNumbers { get; set; }
    }
}
