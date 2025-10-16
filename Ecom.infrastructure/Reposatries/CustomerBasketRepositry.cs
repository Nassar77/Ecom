using Ecom.Core.Entities;
using Ecom.Core.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace Ecom.infrastructure.Reposatries;
public class CustomerBasketRepositry : ICustomerBasketRepositry
{
    private readonly IDatabase _database;
    public CustomerBasketRepositry(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }
    public Task<bool> DeleteBasketAsync(string id)
    {
        return _database.KeyDeleteAsync(id);
    }

    public async Task<CustomerBasket> GetBasketAsync(string id)
    {
        var result=await _database.StringGetAsync(id);
        if(!string.IsNullOrEmpty(result))
        {
            return JsonSerializer.Deserialize<CustomerBasket>(result);
        }
        return null;
    }

    public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
    {
        var _basket=await _database.StringSetAsync(basket.Id,JsonSerializer.Serialize(basket),TimeSpan.FromDays(3));

        if (!_basket)
            return null;

        return await GetBasketAsync(basket.Id);
    }
}
