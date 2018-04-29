using StackExchange.Redis;
using System;

namespace PG.Repository.Cache
{
    public class RedisCacheService : ICacheService, IDisposable
    {
        private readonly Lazy<ConnectionMultiplexer> _lazyConnection;

        private IDatabase Cache => _lazyConnection.Value.GetDatabase();

        public RedisCacheService(string cacheConnection)
        {
            _lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
                {
                    try
                    {
                        return ConnectionMultiplexer.Connect(cacheConnection);
                    }
                    catch (Exception)
                    {
                        if (string.IsNullOrEmpty(cacheConnection))
                            throw new Exception("Cache connection is empty.");
                        throw new Exception($"Failed to connect to redis server: {cacheConnection}.");
                    }
                }
            );
        }
        
        public void Add(string key, string value)
        {
            Cache.StringSet(key, value);
        }

        public string Get(string key)
        {
            return Cache.StringGet(key);
        }
        
        public void Delete(string key)
        {
            Cache.KeyDelete(key);
        }

        public void Dispose()
        {
            _lazyConnection.Value.Dispose();
        }
    }
}
