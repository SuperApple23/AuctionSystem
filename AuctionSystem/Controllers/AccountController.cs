using AuctionSystem.Models;
using AuctionSystem.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuctionSystem.Controllers
{
	public class AccountController : Controller
	{
		private readonly SignInManager<AppUser> _signInManager;
		private readonly UserManager<AppUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_signInManager = signInManager;
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(LoginViewModel loginVM)
		{
			if (ModelState.IsValid)
			{
				var result = await _signInManager.PasswordSignInAsync(loginVM.Email!, loginVM.Password!, loginVM.RememberMe, lockoutOnFailure: false);

				if (result.Succeeded)
				{
					return RedirectToAction("Index", "Home");
				}
				else
				{
					ModelState.AddModelError("", "Email hoặc mật khẩu không chính xác");
					return View(loginVM);
				}
			}
			return View(loginVM);
		}

		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(RegisterViewModel registerVM)
		{
			if (ModelState.IsValid)
			{
				AppUser user = new AppUser()
				{
					FullName = registerVM.FullName,
					Address = registerVM.Address,
					Email = registerVM.Email,
					UserName = registerVM.Email,
					PhoneNumber = registerVM.PhoneNumber
				};

				var result = await _userManager.CreateAsync(user, registerVM.Password!);

				if (result.Succeeded)
				{
					var roleExist = await _roleManager.RoleExistsAsync("User");

					if (!roleExist)
					{
						var role = new IdentityRole("User");
						await _roleManager.CreateAsync(role);
					}

					await _userManager.AddToRoleAsync(user, "User");

					return RedirectToAction("Login", "Account");
				}
				else
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError("", error.Description);
					}

					return View(registerVM);
				}
			}
			return View(registerVM);
		}

		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}
	}
}
