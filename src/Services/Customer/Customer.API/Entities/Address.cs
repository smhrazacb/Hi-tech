using System.Globalization;

namespace Customer.API.Entities
{
    public class Address
    {
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string NearByArea { get; set; }
        public string HouseShopPlotNo { get; set; }
        public string Addressline1 { get; set; }
        public GeoLocation GeoLocation { get; set; }
        public Contact Contact { get; set; }
    }
    public class GeoLocation
    {
        public double Latitude { get; set; }
        public double longitude { get; set; }
    }
}
