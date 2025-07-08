// Repositories/IOrderRepository.cs
using System.Threading.Tasks;

namespace FootCap.Repositories
{
    public interface IOrderRepository
    {
        Task<bool> ConfirmOrderAsync(string userId, int productId, int quantity);
    }
}
