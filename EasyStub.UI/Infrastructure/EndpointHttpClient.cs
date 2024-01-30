
using System.Net.Http.Json;

namespace EasyStub.UI.Infrastructure;
public record EndpointDto(int Id, string Path, string Method, int StatusCode, object Body);

public record EndpointToCreateDto(string Path, string Method, int StatusCode, object Body);

public class EndpointHttpClient
{
    private readonly HttpClient client;
    public EndpointHttpClient(HttpClient client)
    {
        this.client = client;
    }

    public async Task<List<EndpointDto>?> GetEndpointsAsync() =>
        await client.GetFromJsonAsync<List<EndpointDto>>("_admin/endpoints");

    public async Task AddEndpointAsync(EndpointToCreateDto endpoint) =>
        await client.PostAsJsonAsync("_admin/endpoints", endpoint);
}