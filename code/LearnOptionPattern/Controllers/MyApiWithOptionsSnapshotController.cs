using LearnOptionPattern.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LearnOptionPattern.Controllers;

[ApiController]
[Route("[controller]")]
public class MyApiWithOptionsSnapshotController : ControllerBase
{
    private readonly IHttpClientFactory _factory;
    private readonly ExternalApiOptions _options;

    public MyApiWithOptionsSnapshotController(
        IHttpClientFactory factory,
        IOptionsSnapshot<ExternalApiOptions> options)
    {
        _options = options.Value;
        _factory = factory;
    }

    [HttpGet(Name = "GetWeatherForecast03")]
    public async Task<string> Get(string cityName = "Paris")
    {
        var baseUrl = _options.Url;
        var token = _options.Token;

        string url = $"{baseUrl}/ExternalWeatherForecast?token={token}&cityName={cityName}";
        using var client = _factory.CreateClient();

        return await client.GetStringAsync(url);
    }
}