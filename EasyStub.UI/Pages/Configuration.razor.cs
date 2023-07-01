namespace EasyStub.UI.Pages;

using EasyStub.UI.Pages.Endpoint;
using EasyStub.UI.UseCases.GetEndpoints;
using Microsoft.AspNetCore.Components;

public partial class Configuration : ComponentBase
{
    List<UseCases.Endpoint> endpoints;

    List<EndpointList.Action> actions = new() { new("Try it", null, (d) => { Console.WriteLine(d); return Task.CompletedTask; }) };

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