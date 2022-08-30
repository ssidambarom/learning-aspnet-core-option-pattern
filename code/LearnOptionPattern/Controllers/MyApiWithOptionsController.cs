using LearnOptionPattern.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LearnOptionPattern.Controllers;

[ApiController]
[Route("[controller]")]
public class MyApiWithOptionsController : ControllerBase
{
    private readonly IHttpClientFactory _factory;
    private readonly ExternalApiOptions options;

    public MyApiWithOptionsController(
        IHttpClientFactory factory,
        IOptions<ExternalApiOptions> options)
    {
        this.options = options.Value;
        _factory = factory;
    }

    [HttpGet(Name = "GetWeatherForecast02")]
    public async Task<string> Get(string cityName = "Paris")
    {
        var baseUrl = options.Url;
        var token = options.Token;

        string url = $"{baseUrl}/ExternalWeatherForecast?token={token}&cityName={cityName}";
        using var client = _factory.CreateClient();

        return await client.GetStringAsync(url);
    }
}
