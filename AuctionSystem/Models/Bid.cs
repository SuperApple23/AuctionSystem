using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionSystem.Models
{
	public class Bid
	{
		[Key]
		public int Id { get; set; }

		[Required]
		[ForeignKey("AppUserId")]
		public string? AppUserId { get; set; }

		public AppUser? AppUser { get; set; }

		[Required]
		[ForeignKey("AuctionId")]
		public int AuctionId { get; set; }

		public Auction? Auction { get; set; }

		[Required(ErrorMessage = "Chưa nhập {0}")]
		[Display(Name = "Giá đấu tiếp theo")]
		[RegularExpression(@"^\d+(\.\d+)?$", ErrorMessage = "Đây phải là số nguyên")]
		public double BidPrice { get; set; }

		public DateTime? BidDate { get; set; }
	}
}
