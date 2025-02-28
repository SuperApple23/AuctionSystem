using AuctionSystem.Models;
using AuctionSystem.ViewModels;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace AuctionSystem.Mapper
{
	public static class AuctionMapper
	{
		public static AuctionCampaignViewModel ToAuctionCampaign(this Campaign campaign)
		{
			return new AuctionCampaignViewModel()
			{
				CampaignId = campaign.CampaignId,
				CampaignName = campaign.CampaignName,
				Description = campaign.Description,
				StartDateTime = campaign.StartDateTime,
				EndDateTime = campaign.EndDateTime
			};
		}

		public static AuctionProductViewModel ToAuctionProduct(this Auction auction)
		{
			double currentStartingPrice = 0;
			if (auction.Bids == null || auction.Bids.Count == 0)
			{
				currentStartingPrice = auction.StartingPrice;
			}
			else
			{
				currentStartingPrice = auction.Bids.LastOrDefault()!.BidPrice;
			}

			return new AuctionProductViewModel()
			{
				MainImage = auction.Product!.MainImage,
				ProductName = auction.Product.ProductName,
				ListedPrice = auction.Product.ListedPrice,
				StartingPrice = currentStartingPrice,
				MinimumPriceIncrement = auction.MinimumPriceIncrement,
				AuctionId = auction.Id,
				InstantSellPrice = auction.InstantSellPrice
			};
		}
	}
}
