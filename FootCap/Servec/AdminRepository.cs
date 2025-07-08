using FootCap.Models;
using FootCap.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AdminRepository : IAdminRepository
{
    private readonly Context _context;
    private readonly UserManager<User> _userManager;

    public AdminRepository(Context context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<int> GetTotalUsersAsync()
    {
        return await _userManager.Users.CountAsync();
    }

    public async Task<int> GetTotalProductsAsync()
    {
        return await _context.Products.CountAsync();
    }

    public async Task<int> GetTotalOrdersAsync()
    {
        return await _context.Orders.CountAsync();
    }

    public async Task<List<Order>> GetAllOrdersAsync()
    {
        return await _context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .ToListAsync();
    }

    public async Task<Order> GetOrderByIdAsync(int orderId)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.OrderId == orderId);
    }

    public async Task<bool> UpdateOrderAsync(Order updatedOrder)
    {
        var orderInDb = await _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.OrderId == updatedOrder.OrderId);

        if (orderInDb == null)
            return false;

        orderInDb.OrderDate = updatedOrder.OrderDate;
        orderInDb.TotalAmount = updatedOrder.TotalAmount;

        try
        {
            _context.Update(orderInDb);
            await _context.SaveChangesAsync();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteOrderAsync(int orderId)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.OrderId == orderId);

        if (order == null)
            return false;

        _context.OrderItems.RemoveRange(order.OrderItems);
        _context.Orders.Remove(order);

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<UserWithRolesViewModel>> GetAllUsersWithRolesAsync()
    {
        var users = await _userManager.Users.ToListAsync();
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

        return userRolesList;
    }
}
