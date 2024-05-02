using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Application.ViewModels
{
	public class LoginVM
	{
        [Required]
        public string UserName { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(25)]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9_]*$", ErrorMessage = "Invalid password format. Password must start with a letter and can only contain letters, numbers, and underscores.")]
        public string Password { get; set; }
    }
}
