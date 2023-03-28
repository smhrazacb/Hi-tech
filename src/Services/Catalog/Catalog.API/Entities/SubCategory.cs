using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Catalog.API.Entities
{
    public class SubCategory 
    {
        public string SubCategoryName { get; set; }
        public Product Product { get; set; }
    }
}
