using Microsoft.AspNetCore.Identity;

namespace AutoSale.Domain.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; } = null!;

        public string Surname { get; set; } = null!;

        public string LastName { get; set; } = null!;
        
        public int? ImageId { get; set; }

        public Image? Image { get; set; }
    }
}