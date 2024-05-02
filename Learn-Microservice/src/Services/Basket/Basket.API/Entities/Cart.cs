namespace Basket.API.Entities;

public class Cart
{
    public string UserName { get; set; }
    
    public List<CartItem> CartItems { get; set; }

    public Cart()
    {
        CartItems = new List<CartItem>();
    }

    public Cart(string userName)
    {
        UserName = userName;
    }

    public decimal TotalPrice => CartItems.Sum(item => item.Quantity * item.ItemPrice);
}
