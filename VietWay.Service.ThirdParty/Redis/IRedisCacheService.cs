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
    }
}
