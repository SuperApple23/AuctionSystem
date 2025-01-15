using AuctionSystem.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AuctionSystem.ViewModels
{
	public class ProductEditViewModel
	{
		[Key]
		[Column(TypeName = "varchar")]
		[Display(Name = "Mã sản phẩm")]
		[Required(ErrorMessage = "{0} là bắt buộc")]
		[StringLength(50)]
		public required string ProductId { get; set; }

		[Display(Name = "Tên sản phẩm")]
		[Required(ErrorMessage = "{0} là bắt buộc")]
		public required string ProductName { get; set; }

		[Display(Name = "Mô tả")]
		public string? Description { get; set; }

		[Display(Name = "Giá niêm yết")]
		[Required(ErrorMessage = "{0} là bắt buộc")]
		public double ListedPrice { get; set; }

		[Display(Name = "Số lượng")]
		[Required(ErrorMessage = "{0} là bắt buộc")]
		public int Quantity { get; set; }

		public required string ExistingMainImage { get; set; }

		[Display(Name = "Trạng thái")]
		public int? StatusId { get; set; }

		[ForeignKey("StatusId")]
		public Status? Status { get; set; }
	}
}
