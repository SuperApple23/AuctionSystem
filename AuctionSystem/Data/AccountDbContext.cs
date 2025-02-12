using AuctionSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuctionSystem.Data
{
	public class AccountDbContext : IdentityDbContext
	{
		public AccountDbContext(DbContextOptions<AccountDbContext> options) : base(options) { }

		public DbSet<AppUser> AppUsers { get; set; }
	}
}
