using FootCap.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IAccountRepository
{
    Task<User> FindByEmailAsync(string email);
    Task<bool> CheckPasswordSignInAsync(User user, string password, bool isPersistent);
    Task<IList<string>> GetUserRolesAsync(User user);
    Task<IdentityResult> CreateUserAsync(User user, string password);
    Task AddUserToRoleAsync(User user, string role);
    Task SignInAsync(User user, bool isPersistent);
    Task SignOutAsync();
}
