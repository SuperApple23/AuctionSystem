using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionSystem.Data.IdentityMigrations
{
    /// <inheritdoc />
    public partial class UpdateBidDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "BidPrice",
                table: "Bids",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BidPrice",
                table: "Bids");
        }
    }
}
