using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

public class GetCustomerPresenter
{
    [Required]
    public string UserName { get; set; }

    [Required]
    [MinLength(12)]
    [MaxLength(25)]
    [RegularExpression(@"^[a-zA-Z][a-zA-Z0-9_]*$", ErrorMessage = "Password must start with a letter and contain only letters, numbers, and underscores.")]
    public string Password { get; set; }
}
