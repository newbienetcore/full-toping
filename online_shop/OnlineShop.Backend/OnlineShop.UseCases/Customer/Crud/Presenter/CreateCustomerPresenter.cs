using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Application.UseCases.Customer.Crud.Presenter
{
    public class CreateCustomerPresenter
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public int Phonenumber { get; set; }
        [Required]
        public string Password { get; set; }
    }

}
