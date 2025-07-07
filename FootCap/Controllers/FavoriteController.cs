using FootCap.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FootCap.Controllers
{
    [Authorize]
    public class FavoriteController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;

        public FavoriteController(Context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToFavorite(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Json(new { success = false, message = "Please log in first" });
            }

            var exists = await _context.Favorites
                .AnyAsync(f => f.ProductId == productId && f.UserId == user.Id);

            if (!exists)
            {
                _context.Favorites.Add(new Favorite
                {
                    ProductId = productId,
                    UserId = user.Id
                });
                await _context.SaveChangesAsync();
            }

            return Json(new { success = true, message = "Added to favorites successfully" });
        }

        public async Task<IActionResult> ShowFavorites()
        {
            var user = await _userManager.GetUserAsync(User);
            var favorites = await _context.Favorites
                .Include(f => f.Product)
                .Where(f => f.UserId == user.Id)
                .Select(f => f.Product)
                .ToListAsync();

            return View(favorites);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFavorite(int productId)
        {
            var user = await _userManager.GetUserAsync(User);
            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.ProductId == productId && f.UserId == user.Id);

            if (favorite != null)
            {
                _context.Favorites.Remove(favorite);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(ShowFavorites));
        }
    }
}