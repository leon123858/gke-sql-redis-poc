using System.Text.Json;
using NRedisStack;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;

namespace gke_poc.Repository;

public class RedisRepository
{
    private readonly IDatabase _db;
    
    public RedisRepository()
    {
        var host = Environment.GetEnvironmentVariable("REDIS_HOST") ?? "localhost:6379";
        var result = ConnectionMultiplexer.Connect(host);
        _db =  result.GetDatabase();
    }
    
    public async Task<IEnumerable<WeatherForecast>?> GetAsync(string key)
    {
        var result = await _db.StringGetAsync(key);
        return result.IsNullOrEmpty? null : JsonSerializer.Deserialize<IEnumerable<WeatherForecast>>(result);
    }
    
    public async Task<bool> SetAsync(string key, IEnumerable<WeatherForecast> value)
    {
        // value to string
        var json = JsonSerializer.Serialize(value);
        var expiration = TimeSpan.FromSeconds(30);
        return await _db.StringSetAsync(key, json, expiry:expiration);
    }
}