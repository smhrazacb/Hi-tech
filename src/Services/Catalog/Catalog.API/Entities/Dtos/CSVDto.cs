namespace Catalog.API.Entities.Dtos
{
    public class CSVDto
    {
        public int UpdateProductsCount { get;  set; }
        public int NewProductsCount { get;  set; }
        public int InvalidEntriesCount { get;  set; }
        public int DuplicatePartNumbersCount { get;  set; }
        public List<Category> UpdateProducts { get; set; }
        public List<Category> NewProducts { get; set; }
        public List<string> InvalidEntries { get; set; }
        public List<string> DuplicatePartNumbers { get; set; }
    }
}
