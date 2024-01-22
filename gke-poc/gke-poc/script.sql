-- 创建 WeatherForecast 表
CREATE TABLE WeatherForecast
(
    Id SERIAL PRIMARY KEY,
    Date DATE,
    TemperatureC INT,
    Summary VARCHAR(255)
);

-- 插入数据
INSERT INTO WeatherForecast (Date, TemperatureC, Summary)
VALUES
    ('2024-01-22', 20, 'Sunny'),
    ('2024-01-23', 25, 'Cloudy'),
    ('2024-01-24', 18, 'Rainy'),
    ('2024-01-25', 22, 'Snowy'),
    ('2024-01-26', 15, 'Windy');

