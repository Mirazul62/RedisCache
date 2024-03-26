using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace RedisCache.Services
{
    public class CacheService :ICacheService
    {
        private readonly IDistributedCache _cache;
        
        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var value = await _cache.GetStringAsync(key);
            if (value != null)
            {
                return JsonConvert.DeserializeObject<T>(value);
            }
            return default;
        }

        public async Task<T> SetAsync<T>(string key, T value, int AbsoluteExpirationRelativeToNow = 0, int SlidingExpiration = 0)
        {
            AbsoluteExpirationRelativeToNow = AbsoluteExpirationRelativeToNow > 0 ? AbsoluteExpirationRelativeToNow : 2;
            SlidingExpiration = SlidingExpiration > 0 ? SlidingExpiration : 2;

            await _cache.SetStringAsync(key, JsonConvert.SerializeObject(value), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(AbsoluteExpirationRelativeToNow),
                SlidingExpiration = TimeSpan.FromMinutes(SlidingExpiration)
            }); ;
            return value;
        }
        public async Task DeleteAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}
