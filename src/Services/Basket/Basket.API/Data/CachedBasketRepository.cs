using Microsoft.Extensions.Caching.Distributed;
using System.Diagnostics;
using System.Text.Json;

namespace Basket.API.Data
{
    public class CachedBasketRepository
        (IBasketRepository repository, IDistributedCache cache)
        : IBasketRepository
    {
        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
        {
            var stop = new Stopwatch();
            stop.Start();
            var cachedBasket = await cache.GetStringAsync(userName, cancellationToken);

            // Retorna dados que estão no cache, se existirem
            if (!string.IsNullOrEmpty(cachedBasket))
            {
                stop.Stop();
                var teste = stop.Elapsed;
                return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket);

            }

            var basket = await repository.GetBasket(userName, cancellationToken);
            await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);
            
            stop.Stop();
            var teste2 = stop.Elapsed;

            return basket;
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart cart, CancellationToken cancellationToken = default)
        {
            await repository.StoreBasket(cart, cancellationToken);
            await cache.SetStringAsync(cart.UserName, JsonSerializer.Serialize(cart), cancellationToken);

            return cart;
        }

        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            await repository.DeleteBasket(userName, cancellationToken);
            await cache.RemoveAsync(userName, cancellationToken);

            return true;
        }
    }
}