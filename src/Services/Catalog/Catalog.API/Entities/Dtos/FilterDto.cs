using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel;
using static Catalog.API.Entities.Dtos.CEnums;

namespace Catalog.API.Entities.Dtos
{
    public class FilterDto : SortFilterDtoBase
    {
        public PorductAttrib? Filterby { get; set; }
        public string? FilterValue { get; set; }
        public string Filetrbyvalue()
        {
            return GetEnumaValue(Filterby);
        }
    }
    public class SortDto : SortFilterDtoBase
    {
        private PorductAttrib? orderby;
        /// <summary>
        /// Default Value "Stock"
        /// </summary>
        public PorductAttrib? Orderby
        {
            get => orderby;
            set
            {
                if (value == null)
                    orderby = PorductAttrib.Stock;
                orderby = value;
            }
        }
        /// <summary>
        /// Default is Desending order
        /// </summary>
        [DefaultValue(false)]
        public bool IsAccending { get; set; }
        public string Orderbyvalue()
        {
            return GetEnumaValue(Orderby);
        }
    }
    public class SortFilterDto 
    {
        public SortDto Sortdto { get; set; }
        public IEnumerable<FilterDto> Filters { get; set; }
    
    }
}
