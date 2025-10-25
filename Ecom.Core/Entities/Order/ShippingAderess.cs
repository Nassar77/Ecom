namespace Ecom.Core.Entities.Order;

public class ShippingAderess:BaseEntity<int>
{
    public ShippingAderess()
    {
    }

    public ShippingAderess(string firstName, string lastName, string city, string zipCode, string street, string state)
    {
        FirstName = firstName;
        LastName = lastName;
        City = city;
        ZipCode = zipCode;
        Street = street;
        State = state;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }
    public string Street { get; set; }
    public string State { get; set; }
}