using System.ComponentModel.DataAnnotations.Schema;

namespace FootCap.Models
{
    public class Order
    {
        public int OrderId { get; set; }


        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }

        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
     

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
