using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using EasyStub.UI;
using MudBlazor.Services;
using EasyStub.UI.UseCases.Endpoints;
using EasyStub.UI.Infrastructure;
using EasyStub.UI.UseCases.Method;
using EasyStub.UI.UseCases.Fallback;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

Console.WriteLine($"host environment is {builder.HostEnvironment.BaseAddress}");

Console.WriteLine($"api base url is {new Uri(builder.HostEnvironment.BaseAddress + "api/")}");


builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress + "api/") });
builder.Services.AddTransient<EndpointHttpClient>();
builder.Services.AddTransient<GetEndpointsUseCase>();
builder.Services.AddTransient<AddEndpointUseCase>();
builder.Services.AddTransient<GetPossibleMethodsUseCase>();

builder.Services.AddTransient<FallbackHttpClient>();
builder.Services.AddTransient<GetFallbackUseCase>();
builder.Services.AddTransient<FallbackFactory>();

builder.Services.AddMudServices();

await builder.Build().RunAsync();
