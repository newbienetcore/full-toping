using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Application.UseCases.Customer
{
    public class CustomerPresenter
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [MinLength(12)]
        [MaxLength(25)]
        [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9_]*$", ErrorMessage = "Invalid password format. Password must start with a letter and can only contain letters, numbers, and underscores.")]
        public string Password { get; set; }
    }
}
