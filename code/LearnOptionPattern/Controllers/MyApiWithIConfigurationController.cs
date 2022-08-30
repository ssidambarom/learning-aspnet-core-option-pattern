using Microsoft.AspNetCore.Mvc;

namespace LearnOptionPattern.Controllers;

[ApiController]
[Route("[controller]")]
public class MyApiWithIConfigurationController : ControllerBase
{
    private readonly IHttpClientFactory _factory;
    private readonly IConfiguration _configuration;

    public MyApiWithIConfigurationController(
        IHttpClientFactory factory,
        IConfiguration configuration)
    {
        _configuration = configuration;
        _factory = factory;
    }

    [HttpGet(Name = "GetWeatherForecast01")]
    public async Task<string> Get(string cityName = "Paris")
    {
        var baseUrl = _configuration.GetValue<string>("ExternalService:Url");
        var token = _configuration.GetValue<string>("ExternalService:Token");

        string url = $"{baseUrl}/ExternalWeatherForecast?token={token}&cityName={cityName}";
        using var client = _factory.CreateClient();

        return await client.GetStringAsync(url);
    }
}