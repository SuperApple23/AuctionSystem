using AuctionSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionSystem.Data
{
    public class AuctionDbContext : DbContext
    {
        public AuctionDbContext(DbContextOptions<AuctionDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }

        public DbSet<Image> Images { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<SalesMethod> SalesMethods { get; set; }

        public DbSet<Auction> Auctions { get; set; } // Many to Many

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Auction>()
                .HasOne(p => p.Product)
                .WithMany(a => a.Auctions)
                .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<Auction>()
                .HasOne(c => c.Campaign)
                .WithMany(a => a.Auctions)
                .HasForeignKey(c => c.CampaignId);

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
