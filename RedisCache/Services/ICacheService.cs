namespace RedisCache.Services
{
    public interface ICacheService
    {
        Task<T> GetAsync<T>(string key);
        Task<T> SetAsync<T>(string key, T value, int AbsoluteExpirationRelativeToNow = 0, int SlidingExpiration = 0);
        Task DeleteAsync(string key);
    }
}
