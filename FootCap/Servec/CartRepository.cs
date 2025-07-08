using FootCap.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class CartRepository : ICartRepository
{
    private readonly Context _context;

    public CartRepository(Context context)
    {
        _context = context;
    }

    public async Task<Product> GetProductByIdAsync(int productId)
    {
        return await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
    }

    public async Task<Cart> GetCartByUserIdAsync(string userId)
    {
        return await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
    }

    public async Task<CartItem> GetCartItemAsync(int cartId, int productId)
    {
        return await _context.CartItems.FirstOrDefaultAsync(ci => ci.CartId == cartId && ci.ProductId == productId);
    }

    public async Task AddCart(Cart cart)
    {
        await _context.Carts.AddAsync(cart);
    }

    public async Task AddCartItem(CartItem cartItem)
    {
        await _context.CartItems.AddAsync(cartItem);
    }

    public async Task RemoveCartItem(CartItem cartItem)
    {
        _context.CartItems.Remove(cartItem);
    }

    public async Task<List<CartItem>> GetCartItemsWithProductsAsync(int cartId)
    {
        return await _context.CartItems
            .Where(ci => ci.CartId == cartId)
            .Include(ci => ci.Product)
            .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
