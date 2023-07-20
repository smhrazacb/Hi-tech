namespace Catalog.API.Entities
{
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; }
        public SubCategory SubCategory { get; set; }
    }
}
