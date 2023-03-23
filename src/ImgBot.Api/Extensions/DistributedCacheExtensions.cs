using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace ImgBot.Api.Extensions
{
    public static class DistributedCacheExtensions
    {
        public static async Task<T?> GetJsonAsync<T>(this IDistributedCache cache, string key, CancellationToken cancellationToken = default) where T: class
        {
            var bytes = await cache.GetAsync(key, cancellationToken);

            if(bytes == null)
            {
                return null;
            }
            
            return JsonSerializer.Deserialize<T>(new ReadOnlySpan<byte>(bytes));
        }

        public static async Task SetJsonAsync<T>(this IDistributedCache cache, string key, T obj, JsonSerializerOptions? options = default, CancellationToken cancellationToken = default) where T: class
        {
            await cache.SetAsync(key, JsonSerializer.SerializeToUtf8Bytes<T>(obj, options), cancellationToken);
        }

        public static async Task SetJsonAsync<T>(this IDistributedCache cache, string key, T obj,  DistributedCacheEntryOptions cacheOptions, JsonSerializerOptions? options = default, CancellationToken cancellationToken = default) where T: class
        {
            await cache.SetAsync(key, JsonSerializer.SerializeToUtf8Bytes<T>(obj, options), cacheOptions, cancellationToken);
        }

        public static async Task<string?> GetStringAsync(this IDistributedCache cache, string key, CancellationToken cancellationToken)
        {
            var result = await cache.GetAsync(key, cancellationToken);

            if(result == null) return null;

            return Encoding.UTF8.GetString(result);
        }

        public static async Task SetStringAsync(this IDistributedCache cache, string key, string value, CancellationToken cancellationToken)
        {
            await cache.SetAsync(key, Encoding.UTF8.GetBytes(value), cancellationToken);
        }

        public static async Task SetStringAsync(this IDistributedCache cache, string key, string value, DistributedCacheEntryOptions cacheOptions, CancellationToken cancellationToken)
        {
            await cache.SetAsync(key, Encoding.UTF8.GetBytes(value), cacheOptions, cancellationToken);
        }
    }
}