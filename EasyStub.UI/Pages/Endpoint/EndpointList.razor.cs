namespace EasyStub.UI.Pages.Endpoint;

using Microsoft.AspNetCore.Components;

public partial class EndpointList
{

    private readonly string[] headings = { "Path", "Status code" };

    [Parameter]
    public List<UseCases.Endpoint> Endpoints { get; set; }

    public record Action(string? Label = null, string? Icon = null, Func<UseCases.Endpoint, Task>? Callback = null);

    [Parameter]
    public List<Action> Actions { get; set; } = new();


}