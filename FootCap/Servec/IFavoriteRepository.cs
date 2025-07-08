using FootCap.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IFavoriteRepository
{
    Task<bool> AddToFavoriteAsync(string userId, int productId);
    Task<IEnumerable<Product>> GetFavoritesAsync(string userId);
    Task<bool> RemoveFavoriteAsync(string userId, int productId);
}
