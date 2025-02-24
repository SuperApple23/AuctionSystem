using AuctionSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuctionSystem.Data
{
    public class AuctionDbContext : IdentityDbContext
    {
        public AuctionDbContext(DbContextOptions<AuctionDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }

        public DbSet<Image> Images { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<SalesMethod> SalesMethods { get; set; }

        // Identity
		public DbSet<AppUser> AppUsers { get; set; }

		// Many to Many
		public DbSet<Auction> Auctions { get; set; }
		public DbSet<Bid> Bids { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Bids
            modelBuilder.Entity<Bid>()
                .HasOne(b => b.Auction)
                .WithMany(a => a.Bids)
                .HasForeignKey(b => b.AuctionId);

            modelBuilder.Entity<Bid>()
                .HasOne(b => b.AppUser)
                .WithMany(au => au.Bids)
                .HasForeignKey(b => b.AppUserId);

            // Auctions
            modelBuilder.Entity<Auction>()
                .HasOne(a => a.Product)
                .WithMany(p => p.Auctions)
                .HasForeignKey(a => a.ProductId);

            modelBuilder.Entity<Auction>()
                .HasOne(a => a.Campaign)
                .WithMany(c => c.Auctions)
                .HasForeignKey(a => a.CampaignId);

            modelBuilder.Entity<Status>().HasData(
                new Status { Id = 1, Name = "Mở" },
                new Status { Id = 2, Name = "Đóng" },
                new Status { Id = 3, Name = "Hết hàng" });

            modelBuilder.Entity<SalesMethod>().HasData(
                new SalesMethod { Id = 1, Name = "Đấu giá" },
                new SalesMethod { Id = 2, Name = "Flash Sale" });
        }
    }
}
