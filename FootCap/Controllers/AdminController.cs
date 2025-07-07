using FootCap.Models;
using FootCap.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace FootCap.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;

        public AdminController(Context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // لوحة التحكم
        public async Task<IActionResult> AdminDashboard()
        {
            ViewBag.TotalUsers = await _userManager.Users.CountAsync();
            ViewBag.TotalProducts = await _context.Products.CountAsync();
            ViewBag.TotalOrders = await _context.Orders.CountAsync();
            return View();
        }

        // عرض جميع الطلبات
        public async Task<IActionResult> Orders()
        {
            var orders = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .ToListAsync();

            return View(orders);
        }

        // تعديل الطلب (عرض النموذج)
        [HttpGet]
        public async Task<IActionResult> EditOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        // تعديل الطلب (حفظ التعديل)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditOrder(int id, Order updatedOrder)
        {
            if (id != updatedOrder.OrderId)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(updatedOrder);

            var orderInDb = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (orderInDb == null)
                return NotFound();

            // تعديل التاريخ والمبلغ فقط (ببساطة)
            orderInDb.OrderDate = updatedOrder.OrderDate;
            orderInDb.TotalAmount = updatedOrder.TotalAmount;

            try
            {
                _context.Update(orderInDb);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Order updated successfully.";
                return RedirectToAction(nameof(Orders));
            }
            catch
            {
                TempData["Error"] = "Error updating order.";
                return View(updatedOrder);
            }
        }

        public async Task<IActionResult> Users()
        {
            var users = await _userManager.Users.ToListAsync();

            // تجهيز قائمة تحتوي على المستخدم مع أدواره
            var userRolesList = new List<UserWithRolesViewModel>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRolesList.Add(new UserWithRolesViewModel
                {
                    User = user,
                    Roles = roles
                });
            }

            return View(userRolesList);
        }



        // حذف الطلب
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                TempData["Error"] = "Order not found.";
                return RedirectToAction(nameof(Orders));
            }

            _context.OrderItems.RemoveRange(order.OrderItems);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Order deleted successfully.";
            return RedirectToAction(nameof(Orders));
        }

        // تفاصيل مستخدم
       
        
    }
}
