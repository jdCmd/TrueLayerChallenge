using System;
using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using TrueLayerChallenge.WebApi.Dtos;
using Xunit;

namespace TrueLayerChallenge.WebApi.Tests
{
    public class WebApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public WebApiTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        #region Pokemon

        [Fact]
        public async void Pokemon_Get_PokemonNameNotProvided_ReturnsNotFound()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync($"pokemon");

            // Assert
            Assert.NotNull(response);
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("mewthree")]
        [InlineData("blah_blah")]
        public async void Pokemon_Get_PokemonNameNotActualPokemon_ReturnsNotFound(string pokemonName)
        {
            throw new NotImplementedException();

            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync($"pokemon/{pokemonName}");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var reason = await response.Content.ReadAsStringAsync();
            Assert.Equal("Could not find pokemon details for given pokemon name.", reason);
        }

        [Theory]
        [InlineData("charizard", "some description")]
        [InlineData("mewtwo", "some description")]
        [InlineData("snorlax", "some description")]
        public async void Pokemon_Get_PokemonNameIsValid_ReturnsExpectedResult(string pokemonName, string expectedDescription)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync($"pokemon/{pokemonName}");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var dto = JsonConvert.DeserializeObject<ShakespeareanPokemonDescriptionDto>(await response.Content.ReadAsStringAsync());
            Assert.NotNull(dto);
            Assert.Equal(pokemonName, dto.Name);
            Assert.Equal(expectedDescription, dto.Description);
        }

        #endregion Pokemon

    }
}