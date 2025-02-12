using System.ComponentModel.DataAnnotations;

namespace AuctionSystem.ViewModels
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "Bắt buộc phải có {0}")]
		[EmailAddress(ErrorMessage = "{0} không đúng định dạng")]
		public string? Email { get; set; }

		[Required(ErrorMessage = "Bắt buộc phải có {0}")]
		[Display(Name = "Mật khẩu")]
		[DataType(DataType.Password)]
		public string? Password { get; set; }

		[Display(Name = "Ghi nhớ tài khoản")]
		public bool RememberMe { get; set; }
	}
}
