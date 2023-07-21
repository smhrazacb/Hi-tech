using Catalog.API.Filter;
using Catalog.API.Responses;
using Catalog.API.Services;

namespace Catalog.API.Helpers
{
    public static class PaginationHelper
    {
        public static PagedResponse<IEnumerable<T>> CreatePagedReponse<T>(
            IEnumerable<T> pagedData, PaginationFilter validFilter, long totalRecords, 
            Dictionary<string, Dictionary<string, int>> filtersMeta, 
            IUriService uriService, string route)
        {
            var respose = new PagedResponse<IEnumerable<T>>(pagedData, filtersMeta, validFilter.PageNumber, validFilter.PageSize);
            var totalPages = totalRecords / (double)validFilter.PageSize;
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));
            respose.NextPage =
                validFilter.PageNumber >= 1 && validFilter.PageNumber < roundedTotalPages
                ? uriService.GetPageUri(new PaginationFilter(validFilter.PageNumber + 1, validFilter.PageSize), route)
                : null;
            respose.PreviousPage =
                validFilter.PageNumber - 1 >= 1 && validFilter.PageNumber <= roundedTotalPages
                ? uriService.GetPageUri(new PaginationFilter(validFilter.PageNumber - 1, validFilter.PageSize), route)
                : null;
            respose.FirstPage = uriService.GetPageUri(new PaginationFilter(1, validFilter.PageSize), route);
            respose.LastPage = uriService.GetPageUri(new PaginationFilter(roundedTotalPages, validFilter.PageSize), route);
            respose.TotalPages = roundedTotalPages;
            respose.TotalRecords = totalRecords;
            return respose;
        }
    }
}
