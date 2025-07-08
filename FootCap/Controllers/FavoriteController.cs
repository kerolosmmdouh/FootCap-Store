using FootCap.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Authorize]
public class FavoriteController : Controller
{
    private readonly IFavoriteRepository _favoriteRepo;
    private readonly UserManager<User> _userManager;

    public FavoriteController(IFavoriteRepository favoriteRepo, UserManager<User> userManager)
    {
        _favoriteRepo = favoriteRepo;
        _userManager = userManager;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddToFavorite(int productId)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return Json(new { success = false, message = "Please log in first" });

        var added = await _favoriteRepo.AddToFavoriteAsync(user.Id, productId);

        if (added)
            return Json(new { success = true, message = "Added to favorites successfully" });

        return Json(new { success = false, message = "Already in favorites" });
    }

    public async Task<IActionResult> ShowFavorites()
    {
        var user = await _userManager.GetUserAsync(User);
        var favorites = await _favoriteRepo.GetFavoritesAsync(user.Id);
        return View(favorites);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveFavorite(int productId)
    {
        var user = await _userManager.GetUserAsync(User);
        await _favoriteRepo.RemoveFavoriteAsync(user.Id, productId);
        return RedirectToAction(nameof(ShowFavorites));
    }
}
