using System;
using System.Collections.Generic;
using System.Linq;
using EasyStub.UI.UseCases.GetEndpoints;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace EasyStub.UI.Pages;

public partial class TryPage : ComponentBase
{
    private List<UseCases.Endpoint> endpoints;
    private readonly List<Endpoint.EndpointList.Action> listActions;

    public TryPage()
    {
        listActions = new() { new(null, MudBlazor.Icons.Material.Filled.PlayArrow, Try) };
    }

    private string serverResponse;

    [Inject]
    private GetEndpointsUseCase getEndpoints { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await FetchEndpoints();
    }

    private async Task FetchEndpoints()
    {
        endpoints = new();
        endpoints = await getEndpoints.Handle();
    }

    [Inject]
    private HttpClient client { get; set; }

    private async Task Try(UseCases.Endpoint endpoint)
    {
        using var request = new HttpRequestMessage(endpoint.Method, client.BaseAddress + endpoint.Path[1..]);

        using var response = await client.SendAsync(request);

        serverResponse = await response.Content.ReadAsStringAsync();

        Console.WriteLine(serverResponse);

        StateHasChanged();
    }
}