using FootCap.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class FavoriteRepository : IFavoriteRepository
{
    private readonly Context _context;

    public FavoriteRepository(Context context)
    {
        _context = context;
    }

    public async Task<bool> AddToFavoriteAsync(string userId, int productId)
    {
        var exists = await _context.Favorites.AnyAsync(f => f.UserId == userId && f.ProductId == productId);
        if (exists)
            return false;

        var favorite = new Favorite
        {
            UserId = userId,
            ProductId = productId
        };

        _context.Favorites.Add(favorite);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<Product>> GetFavoritesAsync(string userId)
    {
        return await _context.Favorites
            .Where(f => f.UserId == userId)
            .Include(f => f.Product)
            .Select(f => f.Product)
            .ToListAsync();
    }

    public async Task<bool> RemoveFavoriteAsync(string userId, int productId)
    {
        var favorite = await _context.Favorites
            .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);

        if (favorite == null)
            return false;

        _context.Favorites.Remove(favorite);
        await _context.SaveChangesAsync();

        return true;
    }
}
