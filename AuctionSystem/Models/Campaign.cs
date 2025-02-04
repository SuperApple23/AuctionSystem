using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionSystem.Models
{
	public class Campaign
	{
		[Key]
		[Display(Name = "Mã chiến dịch")]
		public int CampaignId { get; set; }

		[StringLength(500)]
		[Display(Name = "Tên chiến dịch")]
		[Required(ErrorMessage = "{0} là bắt buộc")]
		public string? CampaignName { get; set; }

		[Display(Name = "Mô tả")]
		public string? Description { get; set; }

		[Required(ErrorMessage = "Yêu cầu phải có Ngày {0}")]
		[Display(Name = "Bắt đầu")]
		public DateTime StartDateTime { get; set; }

		[Required(ErrorMessage = "Yêu cầu phải có Ngày {0}")]
		[Display(Name = "Kết thúc")]
		public DateTime EndDateTime { get; set; }

		public int SalesMethodId { get; set; }

		[ForeignKey("SalesMethodId")]
		[Display(Name = "Phương thức bán hàng")]
		public SalesMethod? SalesMethod { get; set; }

		public List<Auction> Auctions { get; set; } = new List<Auction>();
	}
}
