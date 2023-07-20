namespace ShoppingAggregator.Models
{
    public class CatalogModel
    {
        public string ProductId { get; set; }//
        public string ProductNameShortdesc { get; set; }//
        public UInt32 Quantity { get; set; }//
        public decimal UnitPrice { get; set; }//
        public string? DataSheetUrl { get; set; }
        public string? PictureUrl { get; set; }//
        public DateTime ModifiedDate { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public string Name { get; set; }
        public string Packaging { get; set; }
        public Dictionary<string, string> AdditionalFields { get; set; }


    }
}
