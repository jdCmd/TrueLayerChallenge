namespace TrueLayerChallenge.WebApi.Configuration;

/// <summary>
/// Contains configuration related constants.
/// </summary>
internal class ConfigConstants
{
    /// <summary>
    /// Minimum connection timeout milliseconds allowed on configurable services.
    /// </summary>
    public const int MinConnectionTimeoutMilliseconds = 100;

    #region Config sections

    /// <summary>
    /// Config section name for the PokeApi.
    /// </summary>
    public const string ConfigSection_PokeApi = "PokeApi";

    /// <summary>
    /// Config section name for the Shakespearean translator.
    /// </summary>
    public const string ConfigSection_ShakespeareanTranslator = "ShakespeareanTranslator";

    #endregion Config sections
}