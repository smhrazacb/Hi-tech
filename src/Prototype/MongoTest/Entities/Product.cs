using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoTest.Entities;
using System.Collections.Generic;

namespace MongoTest.Entities
{
    public class Product
    {
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string ManufacturerPartNo { get; set; }
        public string Series { get; set; }

        [BsonElement("Name")]
        public string Packaging { get; set; }
        public UInt32 Stock { get; set; }
        public UInt32 Price { get; set; }
        public Dictionary<string, string> AdditionalFilerFields { get; set; }
        public string ThumbnailImage { get; set; }
    }
}
