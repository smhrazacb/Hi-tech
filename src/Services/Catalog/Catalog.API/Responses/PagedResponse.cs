using EventBus.Messages.Common;

namespace Catalog.API.Responses

{
    public class PagedResponse<T> : ResponseMessage<T>
    {
        private IEnumerable<T> pagedData;
        private Dictionary<string, Dictionary<string, Dictionary<string, int>>> filtersMeta;

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public Uri FirstPage { get; set; }
        public Uri LastPage { get; set; }
        public Dictionary<string, Dictionary<string, int>> FiltersMeta { get; set; }
        public int TotalPages { get; set; }
        public long TotalRecords { get; set; }
        public Uri NextPage { get; set; }
        public Uri PreviousPage { get; set; }

        public PagedResponse(T data, Dictionary<string, Dictionary<string, int>> additionalFiltersMeta, int pageNumber, int pageSize)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.Data = data;
            this.FiltersMeta = additionalFiltersMeta;
            this.Message = null;
            this.Succeeded = true;
            this.Errors = null;
        }

        public PagedResponse(IEnumerable<T> pagedData, Dictionary<string, Dictionary<string, Dictionary<string, int>>> filtersMeta, int pageNumber, int pageSize)
        {
            this.pagedData = pagedData;
            this.filtersMeta = filtersMeta;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
