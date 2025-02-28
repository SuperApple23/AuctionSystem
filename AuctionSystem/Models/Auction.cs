using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionSystem.Models
{
    public class Auction
    {
        [Key]
        public int Id { get; set; }

		[Required]
		[ForeignKey("ProductId")]
		public string? ProductId { get; set; }

        public Product? Product { get; set; }

		[Required]
		[ForeignKey("CampaignId")]
		public int CampaignId { get; set; }

        public Campaign? Campaign { get; set; }

        [Display(Name ="Giá khởi điểm")]
		[Required(ErrorMessage ="{0} lả bắt buộc")]
		public double StartingPrice { get; set; }

		[Display(Name = "Giá mua ngay")]
		[Required(ErrorMessage = "{0} lả bắt buộc")]
		public double InstantSellPrice { get; set; }

		[Display(Name = "Bước giá tối thiểu")]
		[Required(ErrorMessage = "{0} lả bắt buộc")]
		public double MinimumPriceIncrement { get; set; }

		public ICollection<Bid>? Bids { get; set; }
	}
}
