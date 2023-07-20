using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Catalog.API.Filter
{
    public class PaginationFilter
    {
        private int MaxPageSize = 50;
        [DefaultValue(1)]
        public int PageNumber { get; set; }
        [DefaultValue(10)]
        [Range(1,50)]
        public int PageSize { get; set; }
        public PaginationFilter()
        {
            this.PageNumber = 1;
            this.PageSize = MaxPageSize;
        }
        public PaginationFilter(int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > MaxPageSize ? MaxPageSize : pageSize;
        }
    }
}
