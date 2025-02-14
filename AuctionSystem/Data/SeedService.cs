using AuctionSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace AuctionSystem.Data
{
	public static class SeedService
	{
		public static async Task SeedDatabase(this WebApplication app)
		{
			var scope = app.Services.CreateScope();
			var context = scope.ServiceProvider.GetRequiredService<AccountDbContext>();
			var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
			var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
			var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

			try
			{
				logger.LogInformation("Tạo database nếu chưa tồn tại");
				await context.Database.EnsureCreatedAsync();

				logger.LogInformation("Tạo role Admin và User");
				await AddRoleAsync(roleManager, "Admin");
				await AddRoleAsync(roleManager, "User");

				logger.LogInformation("Tạo tài khoản Admin");
				var adminEmail = "admin@superapple.com";
				if (await userManager.FindByEmailAsync(adminEmail) == null)
				{
					var adminUser = new AppUser()
					{
						FullName = "Admin",
						Email = adminEmail,
						UserName = adminEmail
					};

					var result = await userManager.CreateAsync(adminUser, "Admin@123");
					if (result.Succeeded)
					{
						logger.LogInformation("Thêm được tài khoản admin rồi Good Good");
						await userManager.AddToRoleAsync(adminUser, "Admin");
					}
					else
					{
						logger.LogError("Không tạo được tài khoản admin: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
					}

				}
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Lỗi ngay từ đầu REEEEEEEEEEEEEEEEEEEEEEE!!!");
			}
		}

		private static async Task AddRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
		{
			if (!await roleManager.RoleExistsAsync(roleName))
			{
				var result = await roleManager.CreateAsync(new IdentityRole(roleName));
				if (!result.Succeeded)
				{
					throw new Exception($"Không tạo được role '{roleName}': {string.Join(",", result.Errors.Select(e => e.Description))}");
				}
			}
		}
	}
}
