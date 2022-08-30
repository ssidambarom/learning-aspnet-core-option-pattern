using Microsoft.AspNetCore.Mvc;

namespace ExternalServiceOne.Controllers;

public class ClientStore
{
    public IDictionary<string, bool> AllowedClient { get; set; } = new Dictionary<string, bool>();
}

[ApiController]
[Route("[controller]")]
public class ExternalWeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private static readonly string[] Cities = new[]
    {
        "Paris", "Munich", "London", "Bordeaux", "Madrid", "Barcelona", "Rome", "Bruxelle"
    };

    private readonly IConfiguration _configuration;
    private readonly ClientStore _clientStore;

    public ExternalWeatherForecastController(
        IConfiguration configuration,
        ClientStore clientStore)
    {
        _clientStore = clientStore;
        _configuration = configuration;
        var allowedClientSection = _configuration.GetSection("WhiteList");

        _clientStore.AllowedClient.TryAdd(
            "mytokenIsAllowed",
            allowedClientSection.GetValue<bool>("mytokenIsAllowed", false));

        _clientStore.AllowedClient.TryAdd(
            "mytoken2IsAllowed",
            allowedClientSection.GetValue<bool>("mytoken2IsAllowed", false));
    }

    [HttpGet(Name = "GetWeatherForecastByCity")]
    public IActionResult Get(
        [FromQuery] string token,
        [FromQuery] string? cityName = "Paris")
    {
        if (!_clientStore.AllowedClient.TryGetValue($"{token}IsAllowed", out bool isAllowed) || !isAllowed)
            return Unauthorized();

        string foundCity = Cities.FirstOrDefault(c => c == cityName);

        if (foundCity is null)
            return NotFound();

        return Ok(new WeatherForecast
        {
            Date = DateTime.Now,
            TemperatureC = Random.Shared.Next(-20, 55),


            Summary = Summaries[Random.Shared.Next(Summaries.Length)],
            City = foundCity
        });
    }

    [HttpGet("generate-token")]
    public IActionResult GenrateToken()
    {
        string token = GenerateTokenInternal();
        return Ok(token);
    }

    private string GenerateTokenInternal()
    {
        var token = Guid.NewGuid().ToString("D");

        _clientStore.AllowedClient.Add(token + "IsAllowed", true);

        return token;
    }

    [HttpGet("reset-token/{oldToken}")]
    public IActionResult ResetToken(string oldToken)
    {
        var token = oldToken + "IsAllowed";

        if (!_clientStore.AllowedClient.TryGetValue(token, out bool hasfound) && !hasfound)
            return NotFound();

        // Disabling the old token
        _clientStore.AllowedClient[token] = false;

        // Generating the new token
        var newToken = GenerateTokenInternal();

        return Ok(newToken);
    }
}
