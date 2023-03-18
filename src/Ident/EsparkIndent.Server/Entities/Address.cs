﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace EsparkIndent.Server.Entities
{
    public class Address
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }
        public string Country { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string NearByArea { get; set; }
        public string HouseShopPlotNo { get; set; }
        public string Addressline1 { get; set; }
        public int? GeoDataId { get; set; }
        [ForeignKey("GeoDataId")]
        public virtual GeoData? GeoData { get; set; }
    }
}
