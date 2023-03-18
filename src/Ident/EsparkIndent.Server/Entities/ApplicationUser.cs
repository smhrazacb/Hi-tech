using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EsparkIndent.Server.Entities
{
    public class ApplicationUser : BaseEntity
    {
        public EUserStatus UserStatus { get; set; }
        public EOrderType OrderType { get; set; }
        public int AddressId { get; set; }
        [ForeignKey("AddressId")]
        public virtual Address Address { get; set; }

    }
}
