using AuctionSystem.Models;
using AuctionSystem.ViewModels;

namespace AuctionSystem.Helper
{
	public static class ProductHelper
	{
		public static ProductIndexViewModel ToProductDetail(this Product product)
		{
			return new ProductIndexViewModel()
			{
				ProductId = product.ProductId,
				ProductName = product.ProductName,
				ListedPrice = product.ListedPrice,
				Quantity = product.Quantity,
				Status = product.Status,
				MainImage = product.MainImage,
			};
		}
	}
}
