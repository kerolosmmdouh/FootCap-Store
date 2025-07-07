using FootCap.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace FootCap.Controllers
{
    public class OrderController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;

        public OrderController(Context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmOrder(int ProductId, int Quantity)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var product = await _context.Products.FindAsync(ProductId);
            if (product == null)
            {
                TempData["Error"] = "Product not found.";
                return RedirectToAction("ShowCart", "Cart");
            }

            if (Quantity > product.QuantityInStock)
            {
                TempData["Error"] = "Requested quantity not available in stock.";
                return RedirectToAction("ShowCart", "Cart");
            }

            // Create order
            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = product.Price * Quantity,
                OrderItems = new List<OrderItem>
                {
                    new OrderItem
                    {
                        ProductId = ProductId,
                        Quantity = Quantity,
                        UnitPrice = product.Price
                    }
                }
            };

            // Update product quantity
            product.QuantityInStock -= Quantity;

            // Remove from cart
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart != null)
            {
                var cartItem = await _context.CartItems
                    .FirstOrDefaultAsync(ci => ci.CartId == cart.CartId && ci.ProductId == ProductId);

                if (cartItem != null)
                    _context.CartItems.Remove(cartItem);
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"✅ Order confirmed for {product.Name}, Quantity: {Quantity}.";

            return RedirectToAction("ShowCart", "Cart");
        }
    }
}