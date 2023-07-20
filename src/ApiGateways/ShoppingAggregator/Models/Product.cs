namespace ShoppingAggregator.Models 
{
    public class Product
    {
        public string Manufacturer { get; set; }
        public string ManufacturerPartNo { get; set; }
        public string Series { get; set; }
        public string Name { get; set; }
        public string ProductNameShortdesc { get; set; }
        public string Packaging { get; set; }
        public UInt32 Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public Dictionary<string, string> AdditionalFields { get; set; }
        public string? DataSheetUrl { get; set; }
        public string? ImageUrl { get; set; }
    }
}
