using AuctionSystem.Models;
using AuctionSystem.ViewModels;

namespace AuctionSystem.Mapper
{
	public static class BidMapper
	{
		public static BidUserViewModel ToBidUser(this Bid bid, string fullname)
		{
			return new BidUserViewModel()
			{
				FullName = fullname,
				BidPrice = bid.BidPrice,
				BidDate = bid.BidDate
			};
		}
	}
}
