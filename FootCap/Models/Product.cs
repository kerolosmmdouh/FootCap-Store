using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootCap.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [StringLength(100, ErrorMessage = "Product name must not exceed 100 characters")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description must not exceed 500 characters")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [StringLength(200)]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Quantity in stock is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int QuantityInStock { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public virtual Category? Category { get; set; }

        public virtual ICollection<CartItem>? CartItems { get; set; }
        public virtual ICollection<OrderItem>? OrderItems { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}