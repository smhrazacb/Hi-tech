using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Catalog.API.Entities
{
    public class CategoryDto
    {
        public string CategoryName { get; set; }
        public SubCategory SubCategory { get; set; }
    }
}
