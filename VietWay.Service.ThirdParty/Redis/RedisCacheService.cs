using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace VietWay.Service.ThirdParty.Redis
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _database;

        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _database = _connectionMultiplexer.GetDatabase();
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            RedisValue value = await _database.StringGetAsync(key);
            if (value.HasValue)
            {
                return JsonSerializer.Deserialize<T?>(value.ToString());
            }
            return default;
        }
        public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            await _database.StringSetAsync(key, JsonSerializer.Serialize(value), expiry);
        }
        public async Task RemoveAsync(string key)
        {
            await _database.KeyDeleteAsync(key, CommandFlags.FireAndForget);
        }

        public async Task SetMultipleAsync<T>(Dictionary<string, T> dictionary)
        {
            KeyValuePair<RedisKey, RedisValue>[] keyValues = dictionary
                .Select(x => new KeyValuePair<RedisKey, RedisValue>(x.Key, JsonSerializer.Serialize(x.Value))).ToArray();
            await _database.StringSetAsync(keyValues, When.Always, CommandFlags.FireAndForget);
        }
    }
}
