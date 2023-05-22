namespace Catalog.API.Entities.Dtos
{
    public class FilterResult
    {
        public long TotalRecords { get; set; }
        public IEnumerable<Category> Items { get; set; }
    }
}
