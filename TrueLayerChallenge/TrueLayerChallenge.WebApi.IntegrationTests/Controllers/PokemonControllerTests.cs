using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TrueLayerChallenge.WebApi.Controllers;
using TrueLayerChallenge.WebApi.Dtos;
using Xunit;

namespace TrueLayerChallenge.WebApi.IntegrationTests.Controllers
{
    public class PokemonControllerTests : IDisposable
    {
        #region Setup

        private readonly Mock<ILogger<PokemonController>> _loggerMock;

        public PokemonControllerTests()
        {
            _loggerMock = new Mock<ILogger<PokemonController>>();
        }

        #endregion Setup

        #region Ctor

        [Fact]
        public void Ctor_NullLogger_ThrowsArgumentNullException()
        {
            // Act & Assert
            var e = Assert.Throws<ArgumentNullException>(() => new PokemonController(null));
            Assert.NotNull(e);
            Assert.Equal("logger", e.ParamName);
            Assert.Equal("Value cannot be null. (Parameter 'logger')", e.Message);
        }

        #endregion Ctor

        #region Get

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Get_InvalidPokemonName_ReturnsBadRequest(string pokemonName)
        {
            // Arrange
            var sut = GetPokemonController();

            // Act
            var result = sut.GetShakespeareanDescription(pokemonName);

            // Assert
            Assert.NotNull(result);
            var instance = Assert.IsType<BadRequestResult>(result.Result);
            Assert.NotNull(instance);
            Assert.Equal(400, instance.StatusCode);
        }

        [Theory]
        [InlineData("charizard", "some description")]
        [InlineData("mewtwo", "some description")]
        [InlineData("snorlax", "some description")]
        public void Get_ValidPokemonName_ReturnsExpectedResult(string pokemonName, string expectedDescription)
        {
            // Arrange
            var sut = GetPokemonController();

            // Act
            var result = sut.GetShakespeareanDescription(pokemonName);

            // Assert
            Assert.NotNull(result);
            var instance = Assert.IsType<OkObjectResult>(result.Result);
            Assert.NotNull(instance);
            Assert.Equal(200, instance.StatusCode);
            var value = instance.Value as ShakespeareanPokemonDescriptionDto;
            Assert.NotNull(value);
            Assert.Equal(pokemonName, value.Name);
            Assert.Equal(expectedDescription, value.Description);
        }

        #endregion Get

        #region Cleanup

        public void Dispose()
        {
            _loggerMock.VerifyAll();
        }

        #endregion Cleanup

        #region Private

        private PokemonController GetPokemonController()
        {
            return new PokemonController(_loggerMock.Object);
        }

        #endregion Private
    }
}