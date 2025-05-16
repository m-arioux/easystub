
using System.Net;
namespace EasyStub.UI.Pages.Try;

using Microsoft.AspNetCore.Components;

public partial class Response : ComponentBase
{

    private HttpResponseMessage httpResponse;

    [Parameter]
    public HttpResponseMessage HttpResponse
    {
        get { return httpResponse; }
        set { httpResponse = value; _ = ComputeAsyncValues(value); }
    }

    string content;

    private async Task ComputeAsyncValues(HttpResponseMessage response)
    {
        content = await response.Content.ReadAsStringAsync();
    }



}