using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Tweetinvi.Models;

namespace VietWay.Service.ThirdParty.Redis
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IConnectionMultiplexer _connectionMultiplexer;
        private readonly IDatabase _database;
        private readonly IServer _server;

        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _database = _connectionMultiplexer.GetDatabase();
            _server = _connectionMultiplexer.GetServer(_connectionMultiplexer.GetEndPoints().First());
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

        public async Task<int> CreateIntId(string value, TimeSpan? expiry = null)
        {
            int newId;

            do
            {
                newId = RandomNumberGenerator.GetInt32(int.MaxValue);
            } 
            while ((await _database.StringGetAsync($"int32Id:{newId}")).HasValue);

            await _database.StringSetAsync($"int32Id:{newId}", value, expiry);
            return newId;
        }

        public async Task<string?> GetValueFromIntId(int id)
        {
            return await _database.StringGetAsync($"int32Id:{id}");
        }

        public async Task IncrementAsync(string key)
        {
            _ = await _database.StringIncrementAsync(key);
        }

        public async Task<List<string>> GetMultipleKeyAsync(string pattern)
        {
            var keys = new List<string>();
            long cursor = 0;
            int pageSize = 1000;
            do
            {
                var scan = await _server.ExecuteAsync("SCAN",
                    cursor.ToString(),
                    "MATCH", pattern,
                    "COUNT", pageSize.ToString());

                var result = (RedisResult[])scan;
                cursor = long.Parse((string)result[0]);
                var items = (RedisKey[])result[1];
                keys.AddRange(items.Select(key => (string)key));
            } while (cursor != 0);
            return keys;
        }
    }
}