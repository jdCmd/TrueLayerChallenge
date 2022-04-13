using TrueLayerChallenge.WebApi.Resources;

namespace TrueLayerChallenge.WebApi.Configuration;

/// <summary>
/// Contains PokeApi related configuration.
/// </summary>
internal class PokeApiConfig
{
    private int _connectionTimeoutMilliseconds;
    

    /// <summary>
    /// Url of the PokeApi.
    /// </summary>
    public string Url { get; set; } = null!;

    /// <summary>
    /// Connection timeout milliseconds to use when communicating with the PokeApi.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Value to set is less than minimum allowed value of <see cref="ConfigConstants.MinConnectionTimeoutMilliseconds"/> ms</exception>
    public int ConnectionTimeoutMilliseconds
    {
        get => _connectionTimeoutMilliseconds;
        set
        {
            if (value < ConfigConstants.MinConnectionTimeoutMilliseconds) 
                throw new ArgumentOutOfRangeException(nameof(value), UserMessages.PokeApiConfig_ConnectionTimeoutMilliseconds_Invalid);
            _connectionTimeoutMilliseconds = value;
        }
    }
}