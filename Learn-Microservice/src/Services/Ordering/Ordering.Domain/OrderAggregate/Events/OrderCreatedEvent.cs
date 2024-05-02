namespace Ordering.Domain.OrderAggregate.Events;

public class OrderCreatedEvent
{
    public Guid Id { get; private set; }
    public string UserName { get; private set; }
    public string DocumentNo { get; private set; }
    public string EmailAddress { get; private set; }
    public string TotalPrice { get; private set; }
    public string ShippingAddress { get; private set; }
    public string InvoiceAddress { get; private set; }

    public OrderCreatedEvent(Guid id, string userName, string documentNo, string emailAddress, string totalPrice, string shippingAddress, string invoiceAddress)
    {
        Id = id;
        UserName = userName;
        DocumentNo = documentNo;
        EmailAddress = emailAddress;
        TotalPrice = totalPrice;
        ShippingAddress = shippingAddress;
        InvoiceAddress = invoiceAddress;
    }
}