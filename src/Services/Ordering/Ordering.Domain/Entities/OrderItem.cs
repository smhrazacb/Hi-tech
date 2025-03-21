﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Ordering.Domain.Entities
{
    public class OrderItem
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        public int OrderId { get; private set; }
        public string ProductId { get; set; }
        public string ProductNameShortdesc { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string? PictureUrl { get; set; }

    }
}
