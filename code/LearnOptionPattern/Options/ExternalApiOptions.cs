using System.ComponentModel.DataAnnotations;

namespace LearnOptionPattern.Options;

public class ExternalApiOptions
{
    public const string WeatherServiceConfigurationSectionName = "ExternalService";

    [Required]
    [DataType(DataType.Url)]
    public string Url { get; set; } = "";

    [Required]
    public string Token { get; set; } = "";

    [Range(3, 10)]
    public int RetryAllowed { get; set; } = 5;
}

public interface IService1 { }
public interface IService2 { }
public interface IService3 { }
public interface IService4 { }
public interface IService5 { }
public interface IService6 { }