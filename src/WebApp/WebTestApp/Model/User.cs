using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTestApp.Controllers
{
    public class User : BaseEntity
    {
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public EUserStatus UserStatus { get; set; }
        public string UserFullName { get; set; }
        public EOrderType OrderType { get; set; }
        public int AddressId { get; set; }
        [ForeignKey("AddressId")]
        public virtual Address Address { get; set; }

    }
}
