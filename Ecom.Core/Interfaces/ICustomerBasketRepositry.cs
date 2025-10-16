﻿using Ecom.Core.Entities;

namespace Ecom.Core.Interfaces;
public interface ICustomerBasketRepositry
{
    Task<CustomerBasket> GetBasketAsync(string id);
    Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);
    Task<bool> DeleteBasketAsync(string id);
}
