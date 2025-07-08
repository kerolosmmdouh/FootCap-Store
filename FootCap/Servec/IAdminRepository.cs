using FootCap.Models;
using FootCap.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IAdminRepository
{
    Task<int> GetTotalUsersAsync();
    Task<int> GetTotalProductsAsync();
    Task<int> GetTotalOrdersAsync();

    Task<List<Order>> GetAllOrdersAsync();
    Task<Order> GetOrderByIdAsync(int orderId);
    Task<bool> UpdateOrderAsync(Order order);
    Task<bool> DeleteOrderAsync(int orderId);

    Task<List<UserWithRolesViewModel>> GetAllUsersWithRolesAsync();
}
