using System.ComponentModel.DataAnnotations.Schema;

namespace AutoSale.Domain.Models
{
    public class UserReview
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;
        
        public string Text { get; set; } = null!;

        public string UserIdFrom { get; set; } = null!;
        
        public string UserIdTo { get; set; } = null!;
        
        [ForeignKey("UserIdFrom")]
        public User UserFrom { get; set; } = null!;
        
        [ForeignKey("UserIdTo")]
        public User UserTo { get; set; } = null!;
    }
}