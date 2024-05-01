using Microsoft.Extensions.Caching.Distributed;

namespace CourseTry1.Service.Interfaces
{
    public interface ICasheService
    {
        Task<T> GetData<T>(string key);

        Task<bool> SetData<T>(string key, T value, DateTimeOffset expirationTime);

        Task<bool> RemoveData<T>(string key);
    }
}
