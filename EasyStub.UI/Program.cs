using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using EasyStub.UI;
using MudBlazor.Services;
using EasyStub.UI.UseCases.GetEndpoints;
using EasyStub.UI.Infrastructure;
using EasyStub.UI.UseCases.AddEndpoint;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:3000") });
builder.Services.AddTransient<EndpointHttpClient>();
builder.Services.AddTransient<GetEndpointsUseCase>();
builder.Services.AddTransient<AddEndpointUseCase>();

builder.Services.AddMudServices();

await builder.Build().RunAsync();
