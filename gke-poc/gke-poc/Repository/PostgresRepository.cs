using Npgsql;

namespace gke_poc.Repository;

public class PostgresRepository
{
    private readonly string _connectionString;
    
    public PostgresRepository()
    {
        var secretPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "0000";
        var host = Environment.GetEnvironmentVariable("POSTGRES_HOST") ?? "localhost";
        var databaseName = Environment.GetEnvironmentVariable("POSTGRES_DATABASE") ?? "postgres";
        var userName = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "postgres";
        _connectionString =
            $"Host={host};Username={userName};Password={secretPassword};Database={databaseName}";
    }
    
    public async Task<IEnumerable<WeatherForecast>?> GetAsync()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var cmd = new NpgsqlCommand("SELECT * FROM WeatherForecast", connection);
        await using var reader = await cmd.ExecuteReaderAsync();
        var result = new List<WeatherForecast>();
        while (await reader.ReadAsync())
        {
            result.Add(new WeatherForecast
            {
                Date = reader.GetDateTime(1),
                TemperatureC = reader.GetInt32(2),
                Summary = reader.GetString(3)
            });
        }
        await connection.CloseAsync();
        return result;
    }

    public async void InitTable()
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var cmd = new NpgsqlCommand("CREATE TABLE IF NOT EXISTS WeatherForecast (id SERIAL PRIMARY KEY, date DATE, temperatureC INT, summary VARCHAR(255))", connection);
        await cmd.ExecuteNonQueryAsync();
        await connection.CloseAsync();
    }
    
    public async Task<WeatherForecast?> PostAsync(WeatherForecast weatherForecast)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var cmd = new NpgsqlCommand("INSERT INTO WeatherForecast (date, temperatureC, summary) VALUES (@date, @temperatureC, @summary) RETURNING id", connection);
        cmd.Parameters.AddWithValue("date", weatherForecast.Date);
        cmd.Parameters.AddWithValue("temperatureC", weatherForecast.TemperatureC);
        cmd.Parameters.AddWithValue("summary", weatherForecast.Summary);
        var id = await cmd.ExecuteScalarAsync();
        await connection.CloseAsync();
        return new WeatherForecast
        {
            Date = weatherForecast.Date,
            TemperatureC = weatherForecast.TemperatureC,
            Summary = weatherForecast.Summary
        };
    }

}