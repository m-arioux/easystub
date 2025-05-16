namespace EasyStub.UI.Pages.Endpoint;


using System.Net;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using EasyStub.UI.UseCases.Endpoints;
using EasyStub.UI.UseCases.Method;

public partial class AddEndpoint : ComponentBase
{

    public class Model
    {
        public string? Path { get; set; } = "/";
        public HttpMethod? Method { get; set; }
        public HttpStatusCode? StatusCode { get; set; }
    }

    bool success;
    string[] errors = { };
    MudForm form;
    Model model = new();

    JsonEditor.JsonEditor editor = null;

    public class MisconfiguredException : Exception
    {
        public MisconfiguredException(string component) : base($"Misconfigured {component}")
        {

        }
    }

    [Inject]
    public GetPossibleMethodsUseCase GetPossibleMethods { get; set; } = null;

    private readonly List<HttpStatusCode> possibleStatusCodes =
        Enum.GetValues(typeof(HttpStatusCode)).Cast<HttpStatusCode>().ToList();

    private Task<IEnumerable<HttpStatusCode?>> SearchStatusCodes(string? value)
    {
        var readyValue = (value ?? "").ToLowerInvariant();

        var result = possibleStatusCodes
                .Where(x => Display(x).ToLowerInvariant().Contains(readyValue))
                .Cast<HttpStatusCode?>()
                .ToHashSet()
                .AsEnumerable();

        return Task.FromResult(result);
    }

    private static string Display(HttpStatusCode? statusCode)
    {
        if (statusCode is 0 or null)
            return "";

        return $"{(int)statusCode} {statusCode}";
    }

    [Inject]
    private AddEndpointUseCase addEndpoint { get; set; }

    [Inject]
    private NavigationManager navManager { get; set; }

    [Inject]
    private ISnackbar Snackbar { get; set; }

    private async Task Create()
    {
        var x = new AddEndpointUseCase.Input(
            model.Path,
            model.Method ?? throw new ArgumentNullException(nameof(model.Method)),
            model.StatusCode ?? throw new ArgumentNullException(nameof(model.StatusCode)),
            await editor.GetValueAsync());

        await addEndpoint.Handle(x);

        _ = Snackbar.Add("Endpoint created successfully", Severity.Success);

        navManager.NavigateTo("/admin");
    }
}
