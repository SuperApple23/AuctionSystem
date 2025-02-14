using System.ComponentModel.DataAnnotations;

namespace AuctionSystem.ViewModels
{
	public class RegisterViewModel
	{
		[Required(ErrorMessage = "Bắt buộc phải có {0}")]
		[Display(Name = "Họ tên")]
		public string? FullName { get; set; }

		[Required(ErrorMessage = "Bắt buộc phải có {0}")]
		public string? Email { get; set; }

		[Required(ErrorMessage = "Bắt buộc phải có {0}")]
		[StringLength(40, MinimumLength = 8, ErrorMessage = "{0} có độ dài từ {2} đến {1} kí tự")]
		[Display(Name = "Mật khẩu")]
		[DataType(DataType.Password)]
		[Compare("ComfirmPassword", ErrorMessage = "{0} không trùng khớp")]
		public string? Password { get; set; }

		[Required(ErrorMessage = "Bắt buộc phải có {0}")]
		[Display(Name = "Xác thực mật khẩu")]
		[DataType(DataType.Password)]
		public string? ComfirmPassword { get; set; }

		[Required(ErrorMessage = "Bắt buộc phải có {0}")]
		[Display(Name = "Địa chỉ")]
		public string? Address { get; set; }

		[Required(ErrorMessage = "Bắt buộc phải có {0}")]
		[Display(Name = "Số điện thoại")]
		[DataType(DataType.PhoneNumber)]
		public string? PhoneNumber { get; set; }
	}
}
