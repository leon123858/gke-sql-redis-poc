using gke_poc.Repository;
using Microsoft.AspNetCore.Mvc;

namespace gke_poc.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly RedisRepository _redisRepositoryRepository;
    private readonly PostgresRepository _postgresRepository;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, RedisRepository redisRepositoryRepository, PostgresRepository postgresRepository)
    {
        _logger = logger;
        _redisRepositoryRepository = redisRepositoryRepository;
        _postgresRepository = postgresRepository;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        // get from redis
        var cacheResult = _redisRepositoryRepository.GetAsync("weatherforecast").Result;
        if(cacheResult != null) return cacheResult;
        // wait for 5 seconds, then get from postgres
        Thread.Sleep(5000);
        var dbResult = _postgresRepository.GetAsync().Result;
        if (dbResult == null) throw new Exception("Failed to get weatherforecast");
        var newCache = dbResult.ToList();
        // save to redis
        if (_redisRepositoryRepository.SetAsync("weatherforecast", newCache).Result == false)
            throw new Exception("Failed to set weatherforecast");
        return newCache;
    }
}