using Microsoft.AspNetCore.Components;
using EasyStub.UI.UseCases.Fallback;
using EasyStub.UI.Infrastructure;
using EasyStub.UI.Pages.Endpoint.JsonEditor;

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

    protected override async Task OnParametersSetAsync()
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

            await editor.SetValueAsync(jsonFallback.Json);
        }
    }

    public FallbackForm Form = new();

    [Parameter]
    public EventCallback<UseCases.Fallback.Fallback> FallbackChanged { get; set; }

    [Inject]
    public FallbackFactory FallbackFactory { get; set; }

    JsonEditor editor = null;

    public async Task Update()
    {
        Form.Json = await editor.GetValueAsync();
        var dto = new FallbackDto(Form.Type, Form.StatusCode, Form.Json, Form.BaseUrl);

        var updated = FallbackFactory.Create(dto);

        await FallbackChanged.InvokeAsync(updated);
    }
}
