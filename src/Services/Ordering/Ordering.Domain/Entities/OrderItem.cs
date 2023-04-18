using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Entities
{
    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ProductId { get; set; }
        public string ProductNameShortdesc { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal OldUnitPrice { get; set; }
        public int Quantity { get; set; }
        public string PictureUrl { get; set; }
    }
}
