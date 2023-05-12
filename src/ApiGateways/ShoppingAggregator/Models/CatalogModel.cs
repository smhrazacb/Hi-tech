namespace ShoppingAggregator.Models
{
    public class CatalogModel
    {
        public string Id { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Packaging { get; set; }
        public UInt32 Stock { get; set; }
        public decimal Price { get; set; }
        public Dictionary<string, string> AdditionalFields { get; set; }
        public string? DataSheetUrl { get; set; }
        public string? ImageUrl { get; set; }
    }
}
