using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System;

namespace EsparkIndent.Server.Entities

{
    public class BaseEntity : IdentityUser
    {
        //[Key]
        //public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime UpdatedDate { get; set; }
        public DateTime AddedDate { get; set; }

    }
}
