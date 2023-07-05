namespace EasyStub.UI.Pages.Try;

using EasyStub.UI.UseCases.Method;
using Microsoft.AspNetCore.Components;

public partial class Request : ComponentBase
{
    public class Model
    {
        public string? Path { get; set; }
        public HttpMethod? Method { get; set; }
    }

    bool success;
    string[] errors = { };
    public Model ModelValue { get; set; } = new();

    [Inject]
    public GetPossibleMethodsUseCase GetPossibleMethods { get; set; }

    [Parameter]
    public EventCallback<Model> OnSend { get; set; }

    public async Task Send() => await OnSend.InvokeAsync(ModelValue);
}