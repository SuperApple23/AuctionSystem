using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionSystem.Models
{
    public class Campaign
    {
        [Key]
        public int CampaignId { get; set; }

        [Required]
        [StringLength(500)]
        public required string CampaignName { get; set; }

        public int Description { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }

        public int SalesMethodId { get; set; }

        [ForeignKey("SalesMethodId")]
        public SalesMethod? SalesMethod { get; set; }

        public ICollection<Auction>? Auctions { get; set; }
    }
}
