using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel;
using System.Reflection;

namespace Catalog.API.Entities.Dtos
{
    public class CEnums
    {
        public enum PorductAttrib
        {
            CategoryName,
            SubCategoryName,
            Manufacturer,
            ManufacturerPartNo,
            Name,
            Price,
            Packaging,
            Stock,
            Series,
        }
    }
}
