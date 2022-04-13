using Microsoft.Extensions.Options;
using TrueLayerChallenge.WebApi.Configuration;
using TrueLayerChallenge.WebApi.Extensions;
using TrueLayerChallenge.WebApi.Services;
using TrueLayerChallenge.WebApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add configuration options to the container
builder.Services.Configure<PokeApiConfig>(builder.Configuration.GetSection($"{ConfigConstants.ConfigSection_ExternalServices}:{ConfigConstants.ConfigSection_PokeApi}"));
builder.Services.Configure<FunTranslationsConfig>(builder.Configuration.GetSection($"{ConfigConstants.ConfigSection_ExternalServices}:{ConfigConstants.ConfigSection_FunTranslations}"));

// Add services to the container.
builder.Services.AddSingleton<IPokemonService, PokemonService>();
builder.Services.AddSingleton<IFunTranslationsService, FunTranslationsService>();

// Add HttpClient instances
builder.Services.AddHttpClient<IPokemonService, PokemonService>((sp, client) =>
{
    var config = sp.GetRequiredService<IOptions<PokeApiConfig>>().Value;
    client.BaseAddress = new Uri(config.Url);
    client.Timeout = new TimeSpan(0, 0, 0, config.ConnectionTimeoutMilliseconds);
});
builder.Services.AddHttpClient<IFunTranslationsService, FunTranslationsService>((sp, client) =>
{
    var config = sp.GetRequiredService<IOptions<FunTranslationsConfig>>().Value;
    client.BaseAddress = new Uri(config.Url);
    client.Timeout = new TimeSpan(0, 0, 0, config.ConnectionTimeoutMilliseconds);
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.AddExceptionHandling(app.Logger);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

namespace TrueLayerChallenge.WebApi
{
    public partial class Program { }
}