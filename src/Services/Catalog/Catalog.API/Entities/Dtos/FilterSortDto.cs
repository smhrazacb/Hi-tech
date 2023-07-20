namespace Catalog.API.Entities.Dtos
{
    public class FilterSortDto
    {
        public SortDto Sortdto { get; set; }
        public IEnumerable<FilterDto> Filters { get; set; }
     }
}
