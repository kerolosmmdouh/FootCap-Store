using FootCap.Models;
using FootCap.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FootCap.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminRepository _adminRepository;

        public AdminController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<IActionResult> AdminDashboard()
        {
            ViewBag.TotalUsers = await _adminRepository.GetTotalUsersAsync();
            ViewBag.TotalProducts = await _adminRepository.GetTotalProductsAsync();
            ViewBag.TotalOrders = await _adminRepository.GetTotalOrdersAsync();
            return View();
        }

        public async Task<IActionResult> Orders()
        {
            var orders = await _adminRepository.GetAllOrdersAsync();
            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> EditOrder(int id)
        {
            var order = await _adminRepository.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditOrder(int id, Order updatedOrder)
        {
            if (id != updatedOrder.OrderId)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(updatedOrder);

            bool success = await _adminRepository.UpdateOrderAsync(updatedOrder);

            if (success)
            {
                TempData["Success"] = "Order updated successfully.";
                return RedirectToAction(nameof(Orders));
            }
            else
            {
                TempData["Error"] = "Error updating order.";
                return View(updatedOrder);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            bool success = await _adminRepository.DeleteOrderAsync(id);

            if (success)
                TempData["Success"] = "Order deleted successfully.";
            else
                TempData["Error"] = "Order not found.";

            return RedirectToAction(nameof(Orders));
        }

        public async Task<IActionResult> Users()
        {
            var userRolesList = await _adminRepository.GetAllUsersWithRolesAsync();
            return View(userRolesList);
        }
    }
}
