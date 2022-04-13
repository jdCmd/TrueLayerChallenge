using TrueLayerChallenge.WebApi.Configuration;
using TrueLayerChallenge.WebApi.Services;
using TrueLayerChallenge.WebApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add configuration options to the container
builder.Services.AddOptions<PokeApiConfig>(builder.Configuration[ConfigConstants.ConfigSection_PokeApi]);
builder.Services.AddOptions<ShakespeareanTranslatorConfig>(builder.Configuration[ConfigConstants.ConfigSection_PokeApi]);

// Add services to the container.
builder.Services.AddSingleton<IPokemonService, PokemonService>();
builder.Services.AddSingleton<IShakespeareanConverterService, ShakespeareanConverterService>();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

namespace TrueLayerChallenge.WebApi
{
    public partial class Program { }
}