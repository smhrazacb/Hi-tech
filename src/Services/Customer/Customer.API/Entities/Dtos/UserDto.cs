using Customer.API.Entities.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Customer.API.Entities.Dtos
{
    public class UserDto
    {
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public EUserStatus UserStatus { get; set; }
        public string UserFullName { get; set; }
        public EOrderType OrderType { get; set; }
        public virtual AddressDto Address { get; set; }
    }
}
