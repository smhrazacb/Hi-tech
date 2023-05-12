using ShoppingAggregator.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShoppingAggregator.Services.Interfaces
{
    public interface ICatalogService
    {
        Task<CatalogModel> GetCatalog(string id);
    }
}
