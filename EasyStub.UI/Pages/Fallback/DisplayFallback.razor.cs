using System.Runtime.Serialization;
using System.Net;
using System;
using Microsoft.AspNetCore.Components;
using EasyStub.UI.UseCases.Fallback;
using EasyStub.UI.Infrastructure;

namespace EasyStub.UI.Pages.Fallback;

public partial class DisplayFallback : ComponentBase
{

    public class FallbackForm
    {
        public string? Type { get; set; }
        public int? StatusCode { get; set; }
        public string? Json { get; set; }
        public string? BaseUrl { get; set; }
    }

    [Parameter]
    public UseCases.Fallback.Fallback? Fallback { get; set; }

    protected override void OnParametersSet()
    {
        Form = new();

        if (Fallback is not null)
        {
            Form.Type = Fallback.TypeCode;
        }

        if (Fallback is RedirectFallback redirectFallback)
        {
            Form.BaseUrl = redirectFallback.BaseUrl;
        }

        if (Fallback is JsonFallback jsonFallback)
        {
            Form.Json = jsonFallback.Json;
            Form.StatusCode = (int)jsonFallback.StatusCode;
        }
    }

    public FallbackForm Form = new();

    [Parameter]
    public EventCallback<UseCases.Fallback.Fallback> FallbackChanged { get; set; }

    [Inject]
    public FallbackFactory FallbackFactory { get; set; }

    public async Task Update()
    {
        var dto = new FallbackDto(Form.Type, Form.StatusCode, Form.Json, Form.BaseUrl);

        var updated = FallbackFactory.Create(dto);

        await FallbackChanged.InvokeAsync(updated);
    }
}
