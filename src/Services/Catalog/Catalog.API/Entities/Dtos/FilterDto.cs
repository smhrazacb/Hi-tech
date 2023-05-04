using static Catalog.API.Entities.Dtos.CEnums;

namespace Catalog.API.Entities.Dtos
{
    public class FilterDto
    {
        private OrderBy orderby;

        public bool IsAccending { get; set; }
        public OrderBy Orderby
        {
            get => orderby;
            set
            {
                orderby = value;
                switch (Orderby)
                {
                    case OrderBy.CategoryName:
                        Orderbyvalue = "CategoryName";
                        break;
                    case OrderBy.SubCategoryName:
                        Orderbyvalue = "SubCategory.SubCategoryName";
                        break;
                    case OrderBy.Manufacturer:
                        Orderbyvalue = "SubCategory.Product.Manufacturer";
                        break;
                    case OrderBy.ManufacturerPartNo:
                        Orderbyvalue = "SubCategory.Product.ManufacturerPartNo";
                        break;
                    case OrderBy.Name:
                        Orderbyvalue = "SubCategory.Product.Name";
                        break;
                    case OrderBy.Price:
                        Orderbyvalue = "SubCategory.Product.Price";
                        break;
                    case OrderBy.Packaging:
                        Orderbyvalue = "SubCategory.Product.Packaging";
                        break;
                    case OrderBy.Stock:
                        Orderbyvalue = "SubCategory.Product.Stock";
                        break;
                    case OrderBy.Series:
                        Orderbyvalue = "SubCategory.Product.Series";
                        break;
                    default:
                        break;
                }
            }
        }
        public string Orderbyvalue { get; set; }
        public string FilterItemName { get; set; }
   
    }
}
