using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Customer.API.Entities
{
    public class Contact
    {
        public string CountryCode { get; set; }
        public string MobileNumber { get; set; }
    }
}
