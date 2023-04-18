using Microsoft.AspNetCore.Identity;

namespace EsparkIndent.Server.Entities

{
    public class BaseEntity : IdentityUser
    {
        //[Key]
        //public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime UpdatedDate { get; set; }
        public DateTime AddedDate { get; set; }

    }
}
