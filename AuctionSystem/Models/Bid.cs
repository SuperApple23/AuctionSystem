using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionSystem.Models
{
	public class Bid
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey("AppUserId")]
		public string? AppUserId { get; set; }

		public AppUser? AppUser { get; set; }

		[ForeignKey("AuctionId")]
		public int AuctionId { get; set; }

		public Auction? Auction { get; set; }
	}
}
