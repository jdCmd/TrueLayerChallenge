using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using TrueLayerChallenge.WebApi.Services;
using TrueLayerChallenge.WebApi.Services.Interfaces;
using TrueLayerChallenge.WebApi.UnitTests.Resources;
using Xunit;

namespace TrueLayerChallenge.WebApi.UnitTests.Services;

public class PokemonServiceTests : IDisposable
{
    #region Setup

    private readonly Mock<ILogger<PokemonService>> _loggerMock;
    private readonly Mock<IFunTranslationsService> _funTranslationsServiceMock;
    private readonly Mock<HttpClientHandler> _httpClientHandlerMock;
    private readonly HttpClient _httpClient;

    public PokemonServiceTests()
    {
        _loggerMock = new Mock<ILogger<PokemonService>>();
        _funTranslationsServiceMock = new Mock<IFunTranslationsService>();
        _httpClientHandlerMock = new Mock<HttpClientHandler>();
        _httpClient = new HttpClient(_httpClientHandlerMock.Object)
        {
            BaseAddress = new Uri("https://somewhere.com")
        };
    }

    #endregion Setup

    #region Ctor

    [Fact]
    public void Ctor_NullLogger_ThrowsArgumentNullException()
    {
        // Act & Assert
#pragma warning disable CS8625
        var e = Assert.Throws<ArgumentNullException>(() => new PokemonService(null, It.IsAny<HttpClient>(), It.IsAny<IFunTranslationsService>()));
#pragma warning restore CS8625
        Assert.NotNull(e);
        Assert.Equal("logger", e.ParamName);
        Assert.Equal("Value cannot be null. (Parameter 'logger')", e.Message);
    }

    [Fact]
    public void Ctor_NullHttpClient_ThrowsArgumentNullException()
    {
        // Act & Assert
#pragma warning disable CS8625
        var e = Assert.Throws<ArgumentNullException>(() => new PokemonService(_loggerMock.Object, null, It.IsAny<IFunTranslationsService>()));
#pragma warning restore CS8625
        Assert.NotNull(e);
        Assert.Equal("httpClient", e.ParamName);
        Assert.Equal("Value cannot be null. (Parameter 'httpClient')", e.Message);
    }

    [Fact]
    public void Ctor_NullFunTranslationsService_ThrowsArgumentNullException()
    {
        // Act & Assert
#pragma warning disable CS8625
        var e = Assert.Throws<ArgumentNullException>(() => new PokemonService(_loggerMock.Object, _httpClient, null));
#pragma warning restore CS8625
        Assert.NotNull(e);
        Assert.Equal("funTranslationsService", e.ParamName);
        Assert.Equal("Value cannot be null. (Parameter 'funTranslationsService')", e.Message);
    }

    #endregion Ctor

    #region GetShakespeareanDescriptionAsync

    [Fact]
    public async void GetShakespeareanDescriptionAsync_Disposed_ThrowsObjectDisposedException()
    {
        // Arrange
        var sut = GetPokemonService();
        sut.Dispose();

        // Act & Assert
        await Assert.ThrowsAsync<ObjectDisposedException>(() => sut.GetShakespeareanDescriptionAsync(It.IsAny<string>()));
        _funTranslationsServiceMock.Verify(x => x.Dispose());
        _httpClientHandlerMock.Protected().Verify(
            "SendAsync",
            Times.Never(),
            ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
            ItExpr.IsAny<CancellationToken>());
        _funTranslationsServiceMock.Verify(x => x.ConvertToShakespeareanAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async void GetShakespeareanDescriptionAsync_OnHttpRequestException_ReturnsNull()
    {
        // Arrange
        _httpClientHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException());

        var sut = GetPokemonService();

        // Act
        var result = await sut.GetShakespeareanDescriptionAsync(It.IsAny<string>());

        // Assert
        Assert.Null(result);
        _httpClientHandlerMock.Protected().Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
            ItExpr.IsAny<CancellationToken>());
        _funTranslationsServiceMock.Verify(x => x.ConvertToShakespeareanAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async void GetShakespeareanDescriptionAsync_OnJsonSerializationException_ReturnsNull()
    {
        // Arrange
        using var pokemonResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK
        };

        _httpClientHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(pokemonResponse);

        var sut = GetPokemonService();

        // Act
        var result = await sut.GetShakespeareanDescriptionAsync(It.IsAny<string>());

        // Assert
        Assert.Null(result);
        _httpClientHandlerMock.Protected().Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
            ItExpr.IsAny<CancellationToken>());
        _funTranslationsServiceMock.Verify(x => x.ConvertToShakespeareanAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async void GetShakespeareanDescriptionAsync_PokeApiCallReturnsNonSuccessStatusCode_ReturnsNull()
    {
        // Arrange
        var pokemonName = "ditto";

        using var pokemonResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.NotFound
        };

        _httpClientHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(pokemonResponse);

        var sut = GetPokemonService();

        // Act
        var result = await sut.GetShakespeareanDescriptionAsync(pokemonName);

        // Assert
        Assert.Null(result);
        _httpClientHandlerMock.Protected().Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
            ItExpr.IsAny<CancellationToken>());
        _funTranslationsServiceMock.Verify(x => x.ConvertToShakespeareanAsync(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async void GetShakespeareanDescriptionAsync_ShakespeareanTranslationReturnsNull_ReturnsExpectedResult()
    {
        // Arrange
        var pokemonName = "ditto";
        var translatedDescription = "Capable of copying\nan enemy's genetic\ncode to instantly\ftransform itself\ninto a duplicate\nof the enemy.";

        using var pokemonResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StreamContent(new MemoryStream(TestResources.ditto))

        };

        _httpClientHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(pokemonResponse);

        _funTranslationsServiceMock.Setup(x => x.ConvertToShakespeareanAsync(It.IsAny<string>()))
            .ReturnsAsync((string?)null);

        var sut = GetPokemonService();

        // Act
        var result = await sut.GetShakespeareanDescriptionAsync(pokemonName);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(pokemonName, result?.Name);
        Assert.Equal(translatedDescription, result?.Description);
        _httpClientHandlerMock.Protected().Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
            ItExpr.IsAny<CancellationToken>());
        _funTranslationsServiceMock.Verify(x => x.ConvertToShakespeareanAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async void GetShakespeareanDescriptionAsync_Success_ReturnsExpectedResult()
    {
        // Arrange
        var pokemonName = "ditto";
        var translatedDescription = "where for art thou";

        using var pokemonResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = new StreamContent(new MemoryStream(TestResources.ditto))

        };

        _httpClientHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(pokemonResponse);

        _funTranslationsServiceMock.Setup(x => x.ConvertToShakespeareanAsync(It.IsAny<string>()))
            .ReturnsAsync(translatedDescription);

        var sut = GetPokemonService();

        // Act
        var result = await sut.GetShakespeareanDescriptionAsync(pokemonName);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(pokemonName, result?.Name);
        Assert.Equal(translatedDescription, result?.Description);
        _httpClientHandlerMock.Protected().Verify(
            "SendAsync",
            Times.Exactly(1),
            ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
            ItExpr.IsAny<CancellationToken>());
        _funTranslationsServiceMock.Verify(x => x.ConvertToShakespeareanAsync(It.IsAny<string>()), Times.Once);
    }

    #endregion GetShakespeareanDescriptionAsync

    #region Dispose

    [Fact]
    public void Dispose_Called_Succeeds()
    {
        // Arrange
        var sut = GetPokemonService();

        // Act
        var record = Record.Exception(() => sut.Dispose());

        // Assert
        Assert.Null(record);
        _funTranslationsServiceMock.Verify(x => x.Dispose());
    }

    [Fact]
    public void Dispose_CalledMultipleTimes_Succeeds()
    {
        // Arrange
        var sut = GetPokemonService();

        // Act
        var record = Record.Exception(() =>
        {
            sut.Dispose();
            sut.Dispose();
            sut.Dispose();
            sut.Dispose();
            sut.Dispose();
            sut.Dispose();
            sut.Dispose();
            sut.Dispose();
            sut.Dispose();
            sut.Dispose();
            sut.Dispose();
            sut.Dispose();
            sut.Dispose();
        });

        // Assert
        Assert.Null(record);
        _funTranslationsServiceMock.Verify(x => x.Dispose());
    }

    #endregion Dispose

    #region Cleanup

    public void Dispose()
    {
        _loggerMock.VerifyAll();
        _funTranslationsServiceMock.VerifyAll();
        _httpClient?.Dispose();
    }

    #endregion Cleanup

    #region Private

    private PokemonService GetPokemonService()
    {
        return new PokemonService(_loggerMock.Object, _httpClient, _funTranslationsServiceMock.Object);
    }

    #endregion Private
}