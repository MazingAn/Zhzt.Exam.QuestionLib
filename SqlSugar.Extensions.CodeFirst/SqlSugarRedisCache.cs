using SugarRedis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlSugar.Extensions.CodeFirst
{
    public class SqlSugarRedisCache : ICacheService
    {
        public static SugarRedisClient _service = null!;

        public SqlSugarRedisCache()
        {
            _service = new SugarRedisClient();
        }

        public SqlSugarRedisCache(string redisConn)
        {
            _service = new SugarRedisClient(redisConn);
        }

        public void Add<V>(string key, V value)
        {
            _service.Set(key, value);
        }

        public void Add<V>(string key, V value, int cacheDurationInSeconds)
        {
            _service.Set(key, value, cacheDurationInSeconds);
        }

        public bool ContainsKey<V>(string key)
        {
            return _service.Exists(key);
        }

        public V Get<V>(string key)
        {
            return _service.Get<V>(key);
        }

        public IEnumerable<string> GetAllKey<V>()
        {
            return _service.SearchCacheRegex("SqlSugarDataCache.*");
        }

        public V GetOrCreate<V>(string cacheKey, Func<V> create, int cacheDurationInSeconds = int.MaxValue)
        {
            if (this.ContainsKey<V>(cacheKey))
            {
                var result = this.Get<V>(cacheKey);
                if (result == null)
                {
                    return create();
                }
                else
                {
                    return result;
                }
            }
            else
            {
                var result = create();
                this.Add(cacheKey, result, cacheDurationInSeconds);
                return result;
            }
        }

        public void Remove<V>(string key)
        {
            _service.Remove(key);
        }
    }
}
