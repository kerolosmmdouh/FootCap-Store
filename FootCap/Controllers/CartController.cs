using FootCap.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FootCap.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepo;
        private readonly UserManager<User> _userManager;

        public CartController(ICartRepository cartRepo, UserManager<User> userManager)
        {
            _cartRepo = cartRepo;
            _userManager = userManager;
        }

        public async Task<IActionResult> AddToCart(int id)
        {
            var product = await _cartRepo.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            var userId = _userManager.GetUserId(User);
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                await _cartRepo.AddCart(cart);
                await _cartRepo.SaveChangesAsync();
            }

            var existingItem = await _cartRepo.GetCartItemAsync(cart.CartId, id);
            if (existingItem != null)
                return RedirectToAction("showUser", "Prodc");

            var cartItem = new CartItem
            {
                CartId = cart.CartId,
                ProductId = id,
                Quantity = 1
            };
            await _cartRepo.AddCartItem(cartItem);
            await _cartRepo.SaveChangesAsync();

            return RedirectToAction("showUser", "Prodc");
        }

        public async Task<IActionResult> ShowCart()
        {
            var userId = _userManager.GetUserId(User);
            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            if (cart == null)
                return View(new List<CartItem>());

            var cartItems = await _cartRepo.GetCartItemsWithProductsAsync(cart.CartId);
            return View(cartItems);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromCart(int ProductId)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            if (cart == null)
                return RedirectToAction("ShowCart");

            var cartItem = await _cartRepo.GetCartItemAsync(cart.CartId, ProductId);
            if (cartItem == null)
            {
                TempData["Error"] = "Item not found in your cart";
                return RedirectToAction("ShowCart");
            }

            try
            {
                await _cartRepo.RemoveCartItem(cartItem);
                await _cartRepo.SaveChangesAsync();
            }
            catch
            {
                TempData["Error"] = "Error occurred while removing item from cart";
            }

            return RedirectToAction("ShowCart");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleCart(int ProductId)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var cart = await _cartRepo.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                await _cartRepo.AddCart(cart);
                await _cartRepo.SaveChangesAsync();
            }

            var existingItem = await _cartRepo.GetCartItemAsync(cart.CartId, ProductId);
            if (existingItem != null)
            {
                await _cartRepo.RemoveCartItem(existingItem);
                await _cartRepo.SaveChangesAsync();
            }
            else
            {
                var cartItem = new CartItem
                {
                    CartId = cart.CartId,
                    ProductId = ProductId,
                    Quantity = 1
                };
                await _cartRepo.AddCartItem(cartItem);
                await _cartRepo.SaveChangesAsync();
            }

            return RedirectToAction("showUser", "Prodc");
        }
    }
}
