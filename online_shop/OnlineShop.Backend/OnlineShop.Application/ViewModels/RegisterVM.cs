using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Application.ViewModels
{
	public class RegisterVM
	{
		[Required(ErrorMessage = "Username is required")]
		[StringLength(50, MinimumLength = 3, ErrorMessage = "Username length must be between 3 and 50")]
		public string UserName { get; set; }
		[Required(ErrorMessage = "Phone number is required")]
		[RegularExpression(@"^\d{10}$", ErrorMessage = "Phone number must be 10 digits")]
		public int Phonenumber { get; set; }
		[Required(ErrorMessage = "Password is required")]
		[MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

	}
}
