using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebTestApp.Controllers
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime UpdatedDate { get; set; }
        public DateTime AddedDate { get; set; }

    }
}
