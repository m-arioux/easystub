using System.Net.Http.Json;
using System.Text;

namespace EasyStub.UI.Infrastructure;

public record FallbackInputDto(string? Type, int? StatusCode, object? Json, string? BaseUrl);

public record FallbackDto(string? Type, int? StatusCode, string? Json, string? BaseUrl);

public sealed class FallbackSerializationException : Exception
{
    public FallbackSerializationException() : base("Could not serialize fallback's JSON")
    {

    }
}

public class FallbackHttpClient(HttpClient client)
{
    private readonly HttpClient client = client;

    public async Task<FallbackDto> GetFallback()
    {
        var result = await client.GetFromJsonAsync<FallbackInputDto>("_admin/fallback")
          ?? throw new FallbackSerializationException();

        using var stream = new MemoryStream();

        await System.Text.Json.JsonSerializer.SerializeAsync(stream, result.Json);

        return new FallbackDto(result.Type, result.StatusCode, Encoding.UTF8.GetString(stream.ToArray()), result.BaseUrl);
    }

    public async Task SetFallback(FallbackDto fallback) => await client.PatchAsJsonAsync("_admin/fallback", fallback);
}
