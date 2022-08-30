using LearnOptionPattern.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LearnOptionPattern.Controllers;

[ApiController]
[Route("[controller]")]
public class MyApiWithOptionsMonitorController : ControllerBase
{
    private readonly IHttpClientFactory _factory;
    private ExternalApiOptions options;

    public MyApiWithOptionsMonitorController(
        IHttpClientFactory factory,
        IOptionsMonitor<ExternalApiOptions> options)
    {
        this.options = options.CurrentValue;
        options.OnChange(changedOptions =>
        {
            this.options = changedOptions;
        });
        _factory = factory;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<string> Get(string cityName = "Paris")
    {
        var baseUrl = options.Url;
        var token = options.Token;

        string url = $"{baseUrl}/ExternalWeatherForecast?token={token}&cityName={cityName}";
        using var client = _factory.CreateClient();

        return await client.GetStringAsync(url);
    }
}