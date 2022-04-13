namespace TrueLayerChallenge.WebApi.Services.Interfaces;

/// <summary>
/// Service for handling the conversion to Shakespearean.
/// </summary>
internal interface IShakespeareanConverterService
{
    /// <summary>
    /// Converts the given <see cref="content"/> to Shakespearean.
    /// </summary>
    /// <param name="content"><see cref="string"/> to convert.</param>
    /// <returns><see cref="Task"/> with result containing the converted Shakespearean text.</returns>
    Task<string> ConvertToShakespeareanAsync(string content);
}