using Catalog.API.Filter;

namespace Catalog.API.Services
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationFilter filter, string route);
    }
}
