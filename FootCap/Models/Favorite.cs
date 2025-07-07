using FootCap.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class Favorite
{
    [Key]
    public int FavoriteId { get; set; }

    [ForeignKey("Product")]
    public int ProductId { get; set; }
    public Product Product { get; set; }

    // ربط المستخدم
    [ForeignKey("User")]
    public string UserId { get; set; }
    public User User { get; set; }
}
