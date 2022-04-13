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
    /// Config section for external services.
    /// </summary>
    public const string ConfigSection_ExternalServices = "ExternalServices";

    /// <summary>
    /// Config section name for Fun Translations.
    /// </summary>
    public const string ConfigSection_FunTranslations = "FunTranslations";

    /// <summary>
    /// Config section name for the PokeApi.
    /// </summary>
    public const string ConfigSection_PokeApi = "PokeApi";

    #endregion Config sections
}