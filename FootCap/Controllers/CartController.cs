using FootCap.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FootCap.Controllers
{
    public class CartController : Controller
    {
        private readonly Context _context;

        private readonly UserManager<User> _userManager;

        public CartController(Context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // ✅ إضافة منتج إلى السلة
        public IActionResult AddToCart(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
            if (product == null)
                return NotFound();

            string userId = _userManager.GetUserId(User);

            var cart = _context.Carts.FirstOrDefault(c => c.UserId == userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                _context.SaveChanges();
            }

            // ✅ Check لو المنتج موجود بالفعل في السلة
            var existingItem = _context.CartItems.FirstOrDefault(ci => ci.CartId == cart.CartId && ci.ProductId == id);
            if (existingItem != null)
            {
              
                return RedirectToAction("showUser", "Prodc");
            }

            // ✅ Add جديد لو مش موجود
            var cartItem = new CartItem
            {
                CartId = cart.CartId,
                ProductId = id,
                Quantity = 1
            };
            _context.CartItems.Add(cartItem);
            _context.SaveChanges();

            
            return RedirectToAction("showUser", "Prodc");
        }


      
        public IActionResult ShowCart()
        {
            string userId = _userManager.GetUserId(User);


            var cart = _context.Carts.FirstOrDefault(c => c.UserId == userId);
            if (cart == null)
            {
                return View(new List<CartItem>());
            }

            var cartItems = _context.CartItems
                .Where(ci => ci.CartId == cart.CartId)
                .Include(ci => ci.Product)
                .ToList();

            return View(cartItems);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveFromCart(int ProductId)
        {
            
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
             
                return RedirectToAction("Login", "Account");
            }


            var cart = _context.Carts.FirstOrDefault(c => c.UserId == userId);
            if (cart == null)
            {
              
                return RedirectToAction("ShowCart");
            }

       
            var cartItem = _context.CartItems
                .Include(ci => ci.Product)
                .FirstOrDefault(ci => ci.CartId == cart.CartId && ci.ProductId == ProductId);

            if (cartItem == null)
            {
                TempData["Error"] = "Item not found in your cart";
                return RedirectToAction("ShowCart");
            }

           
            try
            {
                _context.CartItems.Remove(cartItem);
                _context.SaveChanges();
               
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error occurred while removing item from cart";
                
            }

            return RedirectToAction("ShowCart");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleCart(int ProductId)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var cart = _context.Carts.FirstOrDefault(c => c.UserId == userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId };
                _context.Carts.Add(cart);
                _context.SaveChanges();
            }

            var existingItem = _context.CartItems.FirstOrDefault(ci => ci.CartId == cart.CartId && ci.ProductId == ProductId);
            if (existingItem != null)
            {
                _context.CartItems.Remove(existingItem);
                _context.SaveChanges();
     
            }
            else
            {
                var cartItem = new CartItem
                {
                    CartId = cart.CartId,
                    ProductId = ProductId,
                    Quantity = 1
                };
                _context.CartItems.Add(cartItem);
                _context.SaveChanges();
            }

            return RedirectToAction("showUser", "Prodc");
        }

    }
}