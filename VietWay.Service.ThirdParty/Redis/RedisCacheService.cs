using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace VietWay.Service.ThirdParty.Redis
{
    public class RedisCacheService (IConnectionMultiplexer connectionMultiplexer) : IRedisCacheService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer = connectionMultiplexer;

        public async Task<T?> GetAsync<T>(string key)
        {
            var db = _connectionMultiplexer.GetDatabase();
            RedisValue value = await db.StringGetAsync(key);
            if (value.HasValue)
            {
                return JsonSerializer.Deserialize<T?>(value.ToString());
            }
            return default;
        }
        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var db = _connectionMultiplexer.GetDatabase();
            await db.StringSetAsync(key, JsonSerializer.Serialize(value), expiry);
        }
        public async Task RemoveAsync(string key)
        {
            var db = _connectionMultiplexer.GetDatabase();
            await db.KeyDeleteAsync(key);
        }
    }
}
