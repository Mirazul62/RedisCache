using StackExchange.Redis;

namespace RedisCache
{
    public class CacheConfig
    {
        public Redis Redis { get; set; }
        public int AbsoluteExpirationRelativeToNow { get; set; }
        public int SlidingExpiration { get; set; }
    }
    public class Redis
    {
        public string ServerAddress { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
