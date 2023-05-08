using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel;
using static Catalog.API.Entities.Dtos.CEnums;

namespace Catalog.API.Entities.Dtos
{
    public class FilterSortDto
    {
        public SortDto Sortdto { get; set; }
        public IEnumerable<FilterDto> Filters { get; set; }
     }
}
