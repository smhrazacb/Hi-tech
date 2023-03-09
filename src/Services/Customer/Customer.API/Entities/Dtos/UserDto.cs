using Customer.API.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Customer.API.Entities.Dtos
{
    public class UserDto
    {
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
        public string Password { get; set; }
        public EUserStatus UserStatus { get; set; }
        public string UserName { get; set; }
        public EOrderType OrderType { get; set; }
        public virtual AddressDto Address { get; set; }
    }
}
