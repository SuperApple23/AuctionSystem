using AuctionSystem.Models;

namespace AuctionSystem.ViewModels
{
	public class ProductIndexViewModel
	{
		public required string ProductId { get; set; }
		public required string ProductName { get; set; }
		public double ListedPrice { get; set; }
		public int Quantity { get; set; }
		public Status? Status { get; set; }
		public string? MainImage { get; set; }
	}
}
