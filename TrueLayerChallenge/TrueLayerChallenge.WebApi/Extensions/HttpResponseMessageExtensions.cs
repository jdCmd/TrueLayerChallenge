using Newtonsoft.Json;

namespace TrueLayerChallenge.WebApi.Extensions;

internal static class HttpResponseMessageExtensions
{
    public static async Task<T> DeseriliseJsonContentAsync<T>(this HttpResponseMessage response)
    {
        return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync()) 
               ?? throw new JsonSerializationException($"Failed to deserialise response to type '{nameof(T)}");
    }
}