using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace Catalog.API.Entities
{
    public class Product 
    {
        public string Manufacturer { get; set; }
        public string ManufacturerPartNo { get; set; }
        //public string Category { get; set; }
        //public string SubCategory { get; set; }
        public string Series { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }
        public string Packaging { get; set; }
        public UInt32 Stock { get; set; }
        public UInt32 Price { get; set; }
        public Dictionary<string, string> AdditionalFields { get; set; }
        public URL DataSheetUrl { get; set; }
        public URL Image { get; set; }
    }
}
