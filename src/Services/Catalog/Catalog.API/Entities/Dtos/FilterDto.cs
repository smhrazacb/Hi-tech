using System.ComponentModel;
using static Catalog.API.Entities.Dtos.CEnums;

namespace Catalog.API.Entities.Dtos
{
    public class FilterDto : SortFilterDtoBase
    {
        /// <summary>
        /// Default value CategoryName
        /// </summary>
        [DefaultValue(PorductAttrib.CategoryName)]
        public PorductAttrib Filterby { get; set; }
        public string FilterValue { get; set; }
        public string Filetrbyvalue()
        {
            return GetEnumaValue(Filterby);
        }
    }
}
