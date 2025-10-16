using Ecom.Core.Entities;
using Ecom.Core.Interfaces;

namespace Ecom.infrastructure.Reposatries;
public class CustomerBasketRepositry : ICustomerBasketRepositry
{
    public Task<bool> DeleteBasketAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<CustomerBasket> GetBasketAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
    {
        throw new NotImplementedException();
    }
}
