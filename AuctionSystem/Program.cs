using AuctionSystem.Data;
using AuctionSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace AuctionSystem
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionString");

			// Add services to the container.
			builder.Services.AddControllersWithViews();
			builder.Services.AddDbContext<AuctionDbContext>(options => options.UseSqlServer(connectionString));
			builder.Services.AddDbContext<AccountDbContext>(options => options.UseSqlServer(connectionString));

			builder.Services.AddIdentity<AppUser, IdentityRole>()
				.AddEntityFrameworkStores<AccountDbContext>()
				.AddDefaultTokenProviders();

			builder.Services.Configure<IdentityOptions>(options =>
			{
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequiredLength = 8;
				options.Password.RequireUppercase = false;
				options.Password.RequireLowercase = false;

				options.User.RequireUniqueEmail = true;

				options.SignIn.RequireConfirmedAccount = false;
				options.SignIn.RequireConfirmedEmail = false;
				options.SignIn.RequireConfirmedPhoneNumber = false;
			});

			builder.Services.ConfigureApplicationCookie(options =>
			{
				options.AccessDeniedPath = "/Home";
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");

			app.Run();
		}
	}
}
