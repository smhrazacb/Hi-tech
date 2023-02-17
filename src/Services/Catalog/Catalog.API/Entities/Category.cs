using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Catalog.API.Entities
{
    public class Category
    {
        public string CategoryName { get; set; }
        public SubCategory SubCategory { get; set; }
        
    }
}
