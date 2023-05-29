using Catalog.API.Entities.Dtos;

namespace Catalog.API.Services
{
    public interface ICSV2Category
    {
        public CSVDto Read(string path);
    }
}
