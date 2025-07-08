using FootCap.Models;
using FootCap.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class OrderController : Controller
{
    private readonly IOrderRepository _orderRepository;
    private readonly UserManager<User> _userManager;

    public OrderController(IOrderRepository orderRepository, UserManager<User> userManager)
    {
        _orderRepository = orderRepository;
        _userManager = userManager;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ConfirmOrder(int ProductId, int Quantity)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
            return RedirectToAction("Login", "Account");

        var success = await _orderRepository.ConfirmOrderAsync(userId, ProductId, Quantity);
        if (!success)
        {
            TempData["Error"] = "Product not found or insufficient stock.";
            return RedirectToAction("ShowCart", "Cart");
        }

        TempData["Success"] = "✅ Order confirmed.";
        return RedirectToAction("ShowCart", "Cart");
    }
}
