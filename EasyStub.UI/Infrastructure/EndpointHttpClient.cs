namespace EasyStub.UI.Infrastructure;

using System.Net.Http.Json;

public record EndpointDto(string Path, string Method, int StatusCode);

public class EndpointHttpClient
{
    private readonly HttpClient client;
    public EndpointHttpClient(HttpClient client) => this.client = client;

    public async Task<List<EndpointDto>?> GetEndpointsAsync() =>
        await client.GetFromJsonAsync<List<EndpointDto>>("/_admin/endpoints");
}