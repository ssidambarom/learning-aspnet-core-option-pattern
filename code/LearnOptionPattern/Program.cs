using LearnOptionPattern.Options;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient();

// builder.Services.Configure<ExternalApiOptions>(
//     builder.Configuration.GetSection(ExternalApiOptions.WeatherServiceConfigurationSectionName));

builder.Services.AddOptions<ExternalApiOptions>()
    .Bind(builder.Configuration.GetSection(ExternalApiOptions.WeatherServiceConfigurationSectionName))
    .ValidateDataAnnotations();

// builder.Services.AddOptions<ExternalApiOptions>()
//     .Bind(builder.Configuration.GetSection(ExternalApiOptions.WeatherServiceConfigurationSectionName))
//     .ValidateDataAnnotations()
//     .ValidateOnStart();

// builder.Services.AddOptions<ExternalApiOptions>()
//     .Bind(builder.Configuration.GetSection(ExternalApiOptions.WeatherServiceConfigurationSectionName))
//     // .ValidateDataAnnotations()
//     .Validate<IService1, IService2, IService3, IService4, IService5>(
//         (options, IService1, IService2, IService3, IService4, IService5) => true);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

var _options = app.Services.GetRequiredService<IOptions<ExternalApiOptions>>();
/*
// DOES NOT SEEMS TO WORK
// var optionsSnapshot = app.Services.GetRequiredService<IOptionsSnapshot<ExternalApiOptions>>();
// var optionsMonitor = app.Services.GetRequiredService<IOptionsMonitor<ExternalApiOptions>>();
*/


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
