using FootCap.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ICartRepository
{
    Task<Product> GetProductByIdAsync(int productId);
    Task<Cart> GetCartByUserIdAsync(string userId);
    Task<CartItem> GetCartItemAsync(int cartId, int productId);
    Task AddCart(Cart cart);
    Task AddCartItem(CartItem cartItem);
    Task RemoveCartItem(CartItem cartItem);
    Task<List<CartItem>> GetCartItemsWithProductsAsync(int cartId);
    Task SaveChangesAsync();
}
