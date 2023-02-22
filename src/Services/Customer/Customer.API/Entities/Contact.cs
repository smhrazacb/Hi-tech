﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Customer.API.Entities
{
    public class Contact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string CountryCode { get; set; }
        public string MobileNumber { get; set; }
        public string? LandlineNumber { get; set; }
        public virtual Address Address { get; set; }

    }
}
