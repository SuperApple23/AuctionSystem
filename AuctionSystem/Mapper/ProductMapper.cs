using AuctionSystem.Models;
using AuctionSystem.ViewModels;

namespace AuctionSystem.Mapper
{
	public static class ProductMapper
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

		public static ProductEditViewModel ToProductEdit(this Product product)
		{
			return new ProductEditViewModel()
			{
				ProductId = product.ProductId,
				ProductName = product.ProductName,
				ListedPrice = product.ListedPrice,
				Quantity = product.Quantity,
				Description = product.Description,
				StatusId = product.StatusId,
				Status = product.Status,
				ExistingMainImage = product.MainImage!
			};
		}

		public static Product ToProduct(this ProductEditViewModel productEdit)
		{
			return new Product()
			{
				ProductId = productEdit.ProductId,
				ProductName = productEdit.ProductName,
				ListedPrice = productEdit.ListedPrice,
				Quantity = productEdit.Quantity,
				Description = productEdit.Description,
				StatusId = productEdit.StatusId,
				Status = productEdit.Status,
				MainImage = productEdit.ExistingMainImage!,
				MainImageFile = null!
			};
		}
	}
}
