using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace WebTestApp.Controllers
{
    public class Contact
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }
        public string CountryCode { get; set; }
        public string MobileNumber { get; set; }
        public string? LandlineNumber { get; set; }

    }
}
