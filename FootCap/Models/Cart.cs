using System.ComponentModel.DataAnnotations.Schema;

namespace FootCap.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }

        public ICollection<CartItem> CartItems { get; set; }
    }
}
