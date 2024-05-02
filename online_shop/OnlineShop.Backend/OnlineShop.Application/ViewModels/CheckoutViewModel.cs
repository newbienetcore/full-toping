namespace OnlineShop.Application.ViewModels
{
    public class CheckoutViewModel
    {
        public List<CartItemViewModel>? Cart { get; set; }
        public CustomerPlaceOrder CustomerPlaceOrder { get; set; }

    }
}
