using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionSystem.Models
{
	public class Product
	{
		[Key]
		[Column(TypeName = "varchar")]
		[Display(Name = "Mã sản phẩm")]
		[Required(ErrorMessage = "{0} là bắt buộc")]
		[StringLength(50)]
		public string? ProductId { get; set; }

		[Display(Name = "Tên sản phẩm")]
		[Required(ErrorMessage = "{0} là bắt buộc")]
		public string? ProductName { get; set; }

		[Display(Name = "Mô tả")]
		public string? Description { get; set; }

		[Display(Name = "Giá niêm yết")]
		[Required(ErrorMessage = "{0} là bắt buộc")]
		public double ListedPrice { get; set; }

		[Display(Name = "Số lượng")]
		[Required(ErrorMessage = "{0} là bắt buộc")]
		public int Quantity { get; set; }

		public string? MainImage { get; set; }

		[NotMapped]
		[Display(Name = "Hình ảnh chính")]
		[Required(ErrorMessage = "Cần có {0}")]
		public IFormFile? MainImageFile { get; set; }

		[NotMapped]
		[Display(Name = "Các hình ảnh khác")]
		public IFormFileCollection? OtherImageFile { get; set; }

		[Display(Name = "Trạng thái")]
		public int? StatusId { get; set; }

		[ForeignKey("StatusId")]
		public Status? Status { get; set; }

		public ICollection<Auction>? Auctions { get; set; }

		public ICollection<Image>? Images { get; set; }
	}
}
