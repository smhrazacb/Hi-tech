using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebTestApp.Controllers
{
    public class GeoData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get;  set; }
        public double Latitude { get; set; }
        public double longitude { get; set; }
    }
}
