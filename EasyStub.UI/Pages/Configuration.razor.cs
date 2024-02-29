
using EasyStub.UI.Pages.Endpoint;
using EasyStub.UI.UseCases.Endpoints;
using EasyStub.UI.UseCases.Fallback;
using Microsoft.AspNetCore.Components;

namespace EasyStub.UI.Pages;

public partial class Configuration : ComponentBase
{
    List<UseCases.Endpoint> endpoints;
    UseCases.Fallback.Fallback fallback;

    List<EndpointList.Action> actions;

    [Inject]
    private GetEndpointsUseCase getEndpoints { get; set; }

    [Inject]
    private GetFallbackUseCase getFallback { get; set; }

    [Inject]
    private NavigationManager NavManager { get; set; }

    protected override async Task OnInitializedAsync()
    {
        actions = new() { new("Try it", null, TryEndpoint) };

        await FetchEndpoints();
        await FetchFallback();
    }

    async Task FetchEndpoints()
    {
        endpoints = new();
        endpoints = await getEndpoints.Handle();
    }

    async Task FetchFallback()
    {
        fallback = await getFallback.Handle();
    }

    async Task TryEndpoint(UseCases.Endpoint endpoint)
    {
        NavManager.NavigateTo("/test");
    }

    [Inject]
    public SetFallbackUseCase SetFallback { get; set; }

    async Task FallbackChanged(UseCases.Fallback.Fallback changed)
    {
        await SetFallback.Handle(changed);
    }
}
