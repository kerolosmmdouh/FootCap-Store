using FootCap.Models;
using FootCap.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

public class AccountController : Controller
{
    private readonly IAccountRepository _accountRepository;

    public AccountController(IAccountRepository accountRepository)
    {
        _accountRepository = accountRepository;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _accountRepository.FindByEmailAsync(model.Email);
        if (user != null)
        {
            var isSignedIn = await _accountRepository.CheckPasswordSignInAsync(user, model.Password, true);
            if (isSignedIn)
            {
                var roles = await _accountRepository.GetUserRolesAsync(user);
                return RedirectToAction(roles.Contains("Admin") ? "AdminDashboard" : "showUser",
                                        roles.Contains("Admin") ? "Admin" : "Prodc");
            }
        }

        ModelState.AddModelError("", "Invalid email or password");
        return View(model);
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = new User
        {
            UserName = model.Email,
            Email = model.Email,
            FullName = model.FullName,
            Address = model.Address
        };

        var result = await _accountRepository.CreateUserAsync(user, model.Password);
        if (result.Succeeded)
        {
            await _accountRepository.AddUserToRoleAsync(user, "User");
            await _accountRepository.SignInAsync(user, true);
            return RedirectToAction("showUser", "Prodc");
        }

        foreach (var error in result.Errors)
            ModelState.AddModelError("", error.Description);

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _accountRepository.SignOutAsync();
        return RedirectToAction("Login");
    }
}
