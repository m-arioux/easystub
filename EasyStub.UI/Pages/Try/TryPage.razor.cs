using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using EasyStub.UI.UseCases.GetEndpoints;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace EasyStub.UI.Pages.Try;

public partial class TryPage : ComponentBase
{
    private List<UseCases.Endpoint> endpoints;
    private readonly List<Endpoint.EndpointList.Action> listActions;

    private Request request;

    public TryPage()
    {
        listActions = new() { new(null, MudBlazor.Icons.Material.Filled.PlayArrow, Try) };
    }

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

    private HttpResponseMessage response;

    [Inject]
    private HttpClient client { get; set; }

    private async Task Try(UseCases.Endpoint endpoint)
    {
        var model = new Request.Model()
        {
            Method = endpoint.Method,
            Path = endpoint.Path
        };

        request.ModelValue = model;

        StateHasChanged();

        await Send(model);
    }

    private async Task Send(Request.Model model)
    {
        using var request = new HttpRequestMessage(model.Method, client.BaseAddress + model.Path[1..]);

        response = await client.SendAsync(request);

        StateHasChanged();
    }
}