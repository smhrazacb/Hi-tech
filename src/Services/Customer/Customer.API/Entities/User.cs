using System.ComponentModel.DataAnnotations;
using Customer.API.Entities.Enums;

namespace Customer.API.Entities
{
    public class User : BaseEntity
    {
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        //public EUserStatus UserStatus { get; set; }
        public string UserFullName { get; set; }
        //public Address Address { get; set; }
        public bool SameBillingAddress { get; set; }
        //public Address BillingAddress { get; set; }
        //public EOrderType OrderType { get; set; }
    }
}
