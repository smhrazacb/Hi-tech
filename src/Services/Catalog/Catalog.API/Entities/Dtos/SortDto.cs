using static Catalog.API.Entities.Dtos.CEnums;
using System.ComponentModel;
using Catalog.API.Data;
using System.Reflection;

namespace Catalog.API.Entities.Dtos
{
    public class SortDto : SortFilterDtoBase
    {
        private PorductAttrib orderby;
        /// <summary>
        /// Default Value "CategoryName"
        /// </summary>
        [DefaultValue(PorductAttrib.CategoryName)]
        public PorductAttrib Orderby
        {
            get => orderby;
            set
            {
                if (value == null)
                    orderby = PorductAttrib.CategoryName;
                orderby = value;
            }
        }
        [DefaultValue(true)]
        public bool IsAccending { get; set; }
        public string Orderbyvalue()
        {
            return GetEnumaValue(Orderby);
        }
    }
}
