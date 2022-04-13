namespace TrueLayerChallenge.WebApi.Services.Interfaces;

/// <summary>
/// Service for handling fun translations operations.
/// </summary>
internal interface IFunTranslationsService : IDisposable
{
    /// <summary>
    /// Converts the given <see cref="content"/> to Shakespearean.
    /// </summary>
    /// <param name="content"><see cref="string"/> to convert.</param>
    /// <returns><see cref="Task"/> with result containing the converted Shakespearean text.</returns>
    /// <exception cref="ObjectDisposedException">The <see cref="IFunTranslationsService"/> instance has been disposed.</exception>
    Task<string?> ConvertToShakespeareanAsync(string content);

    // could add other translation options such as pig latin etc...
}