using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace EsparkIndent.Server.Entities
{
    public class AddressDto
    {
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string NearByArea { get; set; }
        public string HouseShopPlotNo { get; set; }
        public string Addressline1 { get; set; }
        public virtual GeoDataDto? GeoData { get; set; }
    }
}
