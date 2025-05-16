namespace EasyStub.UI.Infrastructure;

using System.Net.Http.Json;

public record EndpointDto(string Path, string Method, int StatusCode, object Body);

public class EndpointHttpClient
{
    private readonly HttpClient client;
    public EndpointHttpClient(HttpClient client)
    {
        this.client = client;
        Console.WriteLine($"base url httpclient is {client.BaseAddress}");
    }

    public async Task<List<EndpointDto>?> GetEndpointsAsync() =>
        await client.GetFromJsonAsync<List<EndpointDto>>("_admin/endpoints");

    public async Task AddEndpointAsync(EndpointDto endpoint) =>
        await client.PostAsJsonAsync("_admin/endpoints", endpoint);
}