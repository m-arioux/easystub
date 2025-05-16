namespace EasyStub.UI.Pages;

using EasyStub.UI.Pages.Endpoint;
using EasyStub.UI.UseCases.Endpoints;
using Microsoft.AspNetCore.Components;

public partial class Configuration : ComponentBase
{
    List<UseCases.Endpoint> endpoints;

    List<EndpointList.Action> actions;

    [Inject]
    private GetEndpointsUseCase getEndpoints { get; set; }

    [Inject]
    private NavigationManager NavManager { get; set; }

    protected override async Task OnInitializedAsync()
    {
        actions = new() { new("Try it", null, TryEndpoint) };

        await FetchEndpoints();
    }

    async Task FetchEndpoints()
    {
        endpoints = new();
        endpoints = await getEndpoints.Handle();
    }

    async Task TryEndpoint(UseCases.Endpoint endpoint)
    {
        NavManager.NavigateTo("/test");
    }
}
