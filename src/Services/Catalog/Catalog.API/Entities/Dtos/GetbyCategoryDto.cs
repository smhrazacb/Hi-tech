﻿using Catalog.API.Filter;

namespace Catalog.API.Entities.Dtos
{
    public class GetbyItemDto
    {
        public PaginationFilter Paginationfilter { get; set; }
        public FilterSortDto FilterSortdto { get; set; }
    }
    
}
