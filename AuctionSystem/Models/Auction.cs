using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionSystem.Models
{
    public class Auction
    {
        [Key]
        public int Id { get; set; }

        public string? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }

        public int CampaignId { get; set; }

        [ForeignKey("CampaignId")]
        public Campaign? Campaign { get; set; }

        [Required]
        public double StartingPrice { get; set; }

        [Required]
        public double InstantSellPrice { get; set; }

        [Required]
        public double MinimumPriceIncrement { get; set; }
    }
}
