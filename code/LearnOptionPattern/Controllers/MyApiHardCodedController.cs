using Microsoft.AspNetCore.Mvc;

namespace LearnOptionPattern.Controllers;

[ApiController]
[Route("[controller]")]
public class MyApiHardCodedController : ControllerBase
{
    private readonly IHttpClientFactory _factory;
    private readonly IConfiguration _configuration;

    public MyApiHardCodedController(
        IHttpClientFactory factory)
    {
        _factory = factory;
    }

    [HttpGet(Name = "GetWeatherForecast00")]
    public async Task<string> Get(string cityName = "Paris")
    {
        string url = $"https://localhost:7003/ExternalWeatherForecast?token=mytoken&cityName={cityName}";
        using var client = _factory.CreateClient();

        return await client.GetStringAsync(url);
    }
}

