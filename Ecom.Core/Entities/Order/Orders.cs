namespace Ecom.Core.Entities.Order;
public class Orders:BaseEntity<int>
{
    public Orders()
    {
        
    }
    public Orders(string buyerEmail, decimal subTotal, ShippingAderess shippingAderess, DeliveryMethod deliveryMethod, IReadOnlyList<OrderItem> orderItems)
    {
        BuyerEmail = buyerEmail;
        SubTotal = subTotal;
        ShippingAderess = shippingAderess;
        DeliveryMethod = deliveryMethod;
        OrderItems = orderItems;
    }

    public string BuyerEmail { get; set; }
    public decimal SubTotal { get; set; }
    public DateTime OrderDate{ get; set; }
    public ShippingAderess ShippingAderess { get; set; }
    public DeliveryMethod DeliveryMethod { get; set; }
    public IReadOnlyList<OrderItem> OrderItems { get; set; }
    public Status Status { get; set; } = Status.Panding;
    public decimal GetRotal()
    {
        return SubTotal+DeliveryMethod.Price;
    }
}
