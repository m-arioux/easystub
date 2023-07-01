using System.Xml.Schema;
using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using MudBlazor;
using EasyStub.UI.UseCases.AddEndpoint;

namespace EasyStub.UI.Pages.Endpoint;

public partial class AddEndpoint : ComponentBase
{

    public class Model
    {
        public string? Path { get; set; }
        public HttpMethod? Method { get; set; }
        public HttpStatusCode? StatusCode { get; set; }
    }

    bool success;
    string[] errors = { };
    MudForm form;
    Model model = new();

    public class MisconfiguredException : Exception
    {
        public MisconfiguredException(string component) : base($"Misconfigured {component}")
        {

        }
    }

    private readonly List<HttpMethod> possibleMethods =
        typeof(HttpMethod)
            .GetProperties()
            .Where(x => x.PropertyType == typeof(HttpMethod))
            .Select(x => (HttpMethod?)x.GetValue(null))
            .Select(x => x ?? throw new MisconfiguredException("PossibleMethods"))
            .ToList();

    private readonly List<HttpStatusCode> possibleStatusCodes =
        Enum.GetValues(typeof(HttpStatusCode)).Cast<HttpStatusCode>().ToList();

    private Task<IEnumerable<HttpStatusCode?>> SearchStatusCodes(string value)
    {
        var readyValue = value.ToLowerInvariant();

        var result = possibleStatusCodes
                .Where(x => Display(x).ToLowerInvariant().Contains(value))
                .Cast<HttpStatusCode?>()
                .ToHashSet()
                .AsEnumerable();

        return Task.FromResult(result
            );
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
            null);

        await addEndpoint.Handle(x);

        Snackbar.Add("Endpoint created successfully", Severity.Success);

        navManager.NavigateTo("/admin");
    }
}