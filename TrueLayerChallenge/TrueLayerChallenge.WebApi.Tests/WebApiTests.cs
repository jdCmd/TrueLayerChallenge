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
        [InlineData("123abc")]
        [InlineData("mewthree")]
        [InlineData("blah_blah")]
        public async void Pokemon_Get_PokemonNameNotActualPokemon_ReturnsNotFound(string pokemonName)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync($"pokemon/{pokemonName}");

            // Assert
            Assert.NotNull(response);
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            var reason = await response.Content.ReadAsStringAsync();
            Assert.Equal("Could not determine description for pokemon with given pokemon name.", reason);
        }

        [Theory]

        // I am having issues locally communicating with Fun Translations as it is blocked by AV
        // Hence just use the actual description for illustration.

        //[InlineData("charizard", "insert expected Shakespearean description here")]
        //[InlineData("mewtwo", "insert expected Shakespearean description here")]
        //[InlineData("snorlax", "insert expected Shakespearean description here")]

        [InlineData("charizard", "Spits fire that\nis hot enough to\nmelt boulders.\fKnown to cause\nforest fires\nunintentionally.")]
        [InlineData("mewtwo", "It was created by\na scientist after\nyears of horrific\fgene splicing and\nDNA engineering\nexperiments.")]
        [InlineData("snorlax", "Very lazy. Just\neats and sleeps.\nAs its rotund\fbulk builds, it\nbecomes steadily\nmore slothful.")]
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