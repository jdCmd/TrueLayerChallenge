using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TrueLayerChallenge.WebApi.Controllers;
using TrueLayerChallenge.WebApi.Dtos;
using TrueLayerChallenge.WebApi.Services.Interfaces;
using Xunit;

namespace TrueLayerChallenge.WebApi.UnitTests.Controllers
{
    public class PokemonControllerTests : IDisposable
    {
        #region Setup

        private readonly Mock<ILogger<PokemonController>> _loggerMock;
        private readonly Mock<IPokemonService> _pokemonServiceMock;

        public PokemonControllerTests()
        {
            _loggerMock = new Mock<ILogger<PokemonController>>();
            _pokemonServiceMock = new Mock<IPokemonService>();
        }

        #endregion Setup

        #region Ctor

        [Fact]
        public void Ctor_NullLogger_ThrowsArgumentNullException()
        {
            // Act & Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var e = Assert.Throws<ArgumentNullException>(() => new PokemonController(null, It.IsAny<IPokemonService>()));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            Assert.NotNull(e);
            Assert.Equal("logger", e.ParamName);
            Assert.Equal("Value cannot be null. (Parameter 'logger')", e.Message);
        }

        [Fact]
        public void Ctor_NullPokemonService_ThrowsArgumentNullException()
        {
            // Act & Assert
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            var e = Assert.Throws<ArgumentNullException>(() => new PokemonController(_loggerMock.Object, null));
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
            Assert.NotNull(e);
            Assert.Equal("pokemonService", e.ParamName);
            Assert.Equal("Value cannot be null. (Parameter 'pokemonService')", e.Message);
        }

        #endregion Ctor

        #region Get

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async void Get_InvalidPokemonName_ReturnsBadRequest(string pokemonName)
        {
            // Arrange
            var sut = GetPokemonController();

            // Act
            var result = await sut.GetShakespeareanDescription(pokemonName);

            // Assert
            Assert.NotNull(result);
            var instance = Assert.IsType<BadRequestResult>(result.Result);
            Assert.NotNull(instance);
            Assert.Equal(400, instance.StatusCode);
            _pokemonServiceMock.Verify(x => x.GetShakespeareanDescriptionAsync(pokemonName), Times.Never);
        }

        [Fact]
        public async void Get_PokemonServiceReturnsNull_ReturnsNotFound()
        {
            // Arrange
            var pokemonName = "ditto";

            _pokemonServiceMock.Setup(x => x.GetShakespeareanDescriptionAsync(pokemonName))
                .ReturnsAsync((ShakespeareanPokemonDescriptionDto?)null);

            var sut = GetPokemonController();

            // Act
            var result = await sut.GetShakespeareanDescription(pokemonName);

            // Assert
            Assert.NotNull(result);
            var instance = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.NotNull(instance);
            Assert.Equal(404, instance.StatusCode);
            Assert.Equal("Could not determine description for pokemon with given pokemon name.", instance.Value);
            _pokemonServiceMock.Verify(x => x.GetShakespeareanDescriptionAsync(pokemonName), Times.Once);
        }

        [Theory]
        [InlineData("charizard", "some description")]
        [InlineData("mewtwo", "some description")]
        [InlineData("snorlax", "some description")]
        public async void Get_ValidPokemonName_ReturnsExpectedResult(string pokemonName, string expectedDescription)
        {
            // Arrange
            var dto = new ShakespeareanPokemonDescriptionDto
            {
                Name = pokemonName,
                Description = expectedDescription
            };

            _pokemonServiceMock.Setup(x => x.GetShakespeareanDescriptionAsync(pokemonName))
                .ReturnsAsync(dto);

            var sut = GetPokemonController();

            // Act
            var result = await sut.GetShakespeareanDescription(pokemonName);

            // Assert
            Assert.NotNull(result);
            var instance = Assert.IsType<OkObjectResult>(result.Result);
            Assert.NotNull(instance);
            Assert.Equal(200, instance.StatusCode);
            var value = instance.Value as ShakespeareanPokemonDescriptionDto;
            Assert.NotNull(value);
#pragma warning disable CS8602
            Assert.Equal(pokemonName, value.Name);
            Assert.Equal(expectedDescription, value.Description);
#pragma warning restore CS8602
            _pokemonServiceMock.Verify(x => x.GetShakespeareanDescriptionAsync(pokemonName), Times.Once);
        }

        #endregion Get

        #region Cleanup

        public void Dispose()
        {
            _loggerMock.VerifyAll();
            _pokemonServiceMock.VerifyAll();
        }

        #endregion Cleanup

        #region Private

        private PokemonController GetPokemonController()
        {
            return new PokemonController(_loggerMock.Object, _pokemonServiceMock.Object);
        }

        #endregion Private
    }
}