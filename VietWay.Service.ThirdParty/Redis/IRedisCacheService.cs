using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VietWay.Service.ThirdParty.Redis
{
    public interface IRedisCacheService
    {
        public Task<T?> GetAsync<T>(string key);
        public Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
        public Task RemoveAsync(string key);
        public Task SetMultipleAsync<T>(Dictionary<string, T> keyValues);
        public Task<int> CreateIntId(string value, TimeSpan? expiry = null);
        public Task<string?> GetValueFromIntId(int id);
        public Task IncrementAsync(string key);
        public Task<List<string>> GetMultipleKeyAsync(string pattern);
    }
}
