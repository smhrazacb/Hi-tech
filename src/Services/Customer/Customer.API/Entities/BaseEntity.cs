using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Customer.API.Entities
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
        public DateTime AddedDate { get; set; } = DateTime.UtcNow;

    }
}
