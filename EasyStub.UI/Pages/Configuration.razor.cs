namespace EasyStub.UI.Pages;

using EasyStub.UI.UseCases.GetEndpoints;
using Microsoft.AspNetCore.Components;

public record Endpoint(string path, int statusCode);

public partial class Configuration : ComponentBase
{
    string[] headings = { "Path", "Status code", "Actions" };

    List<UseCases.Endpoint> endpoints;

    [Inject]
    private GetEndpointsUseCase getEndpoints { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await FetchEndpoints();
    }

    async Task FetchEndpoints()
    {
        endpoints = new();
        endpoints = await getEndpoints.Handle();
    }

}