using Microsoft.AspNetCore.Components;

namespace EasyStub.UI.Pages.Fallback;

public partial class DisplayFallback : ComponentBase
{
    [Parameter]
    public UseCases.Fallback.Fallback? Fallback { get; set; }
}
