using TrueLayerChallenge.WebApi.Resources;

namespace TrueLayerChallenge.WebApi.Configuration;

/// <summary>
/// Contains Fun Translations related configuration.
/// </summary>
internal class FunTranslationsConfig
{
    private int _connectionTimeoutMilliseconds;

    /// <summary>
    /// Url of the Fun Translations api.
    /// </summary>
    public string Url { get; set; } = null!;

    /// <summary>
    /// Connection timeout milliseconds to use when communicating with the Fun Translations api.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Value to set is less than minimum allowed value of <see cref="ConfigConstants.MinConnectionTimeoutMilliseconds"/> ms</exception>
    public int ConnectionTimeoutMilliseconds
    {
        get => _connectionTimeoutMilliseconds;
        set
        {
            if (value < ConfigConstants.MinConnectionTimeoutMilliseconds)
                throw new ArgumentOutOfRangeException(nameof(value),  string.Format(UserMessages.FunTranslationsConfig_ConnectionTimeoutMilliseconds_Invalid, ConfigConstants.MinConnectionTimeoutMilliseconds));
            _connectionTimeoutMilliseconds = value;
        }
    }
}