using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel;
using static Catalog.API.Entities.Dtos.CEnums;

namespace Catalog.API.Entities.Dtos
{
    public class FilterDto
    {
        private PorductAttrib? orderby;
        public PorductAttrib? Filterby { get; set; }
        [DefaultValue("")]
        public string FilterValue { get; set; }
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
        public string Filetrbyvalue()
        {
            return GetEnumaValue(Filterby);
        }

        public string Orderbyvalue()
        {
            return GetEnumaValue(Orderby);
        }
        private string GetEnumaValue(PorductAttrib? porductAttrib)
        {
            switch (porductAttrib)
            {
                case PorductAttrib.CategoryName:
                    return "CategoryName";

                case PorductAttrib.SubCategoryName:
                    return "SubCategory.SubCategoryName";

                case PorductAttrib.Manufacturer:
                    return "SubCategory.Product.Manufacturer";

                case PorductAttrib.ManufacturerPartNo:
                    return "SubCategory.Product.ManufacturerPartNo";

                case PorductAttrib.Name:
                    return "SubCategory.Product.Name";

                case PorductAttrib.Price:
                    return "SubCategory.Product.Price";

                case PorductAttrib.Packaging:
                    return "SubCategory.Product.Packaging";

                case PorductAttrib.Stock:
                    return "SubCategory.Product.Stock";

                case PorductAttrib.Series:
                    return "SubCategory.Product.Series";

            }
            return "SubCategory.Product.Name";
        }
    }
}
