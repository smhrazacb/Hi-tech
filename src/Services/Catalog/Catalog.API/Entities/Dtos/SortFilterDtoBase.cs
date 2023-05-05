using static Catalog.API.Entities.Dtos.CEnums;

namespace Catalog.API.Entities.Dtos
{
    public class SortFilterDtoBase
    {
        protected string GetEnumaValue(PorductAttrib? porductAttrib)
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