using CourseTry1.Service.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis;
using System.Text;
using System.Text.Json;

namespace CourseTry1.Service.Implementations
{
    public class CasheService : ICasheService
    {
        private readonly StackExchange.Redis.IDatabase cache;
        
        public CasheService(IConfiguration configuration)
        {
            var redis = ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection"));

            this.cache = redis.GetDatabase();
        }

        public async Task<T> GetData<T>(string key)
        {
            var value = await cache.StringGetAsync(key);  
        
            if(!string.IsNullOrEmpty(value))
            {
                return JsonSerializer.Deserialize<T>(value);
            }

            return default;
        }

        public async Task<bool> RemoveData<T>(string key)
        {
            var exist = await cache.KeyExistsAsync(key);
        
            return exist ? await cache.KeyDeleteAsync(key) : false;   
        }

        public async Task<bool> SetData<T>(string key, T value, DateTimeOffset expirationTime)
        {
            var lifeTime = expirationTime.DateTime.Subtract(DateTime.UtcNow);

            return await cache.StringSetAsync(key, JsonSerializer.Serialize<T>(value), lifeTime); ;
        }
    }
}
