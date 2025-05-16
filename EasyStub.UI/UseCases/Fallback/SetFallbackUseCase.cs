
using EasyStub.UI.Infrastructure;

namespace EasyStub.UI.UseCases.Fallback;

public class SetFallbackUseCase(FallbackHttpClient client) : IUseCase
{
    private readonly FallbackHttpClient client = client;

    public async Task Handle(Fallback fallback)
    {
        // TODO: we already do this somewhere else, make injectable reusable code
        var dto = new FallbackDto(fallback.TypeCode, null, null, null);

        if (fallback is JsonFallback jsonFallback)
        {
            dto = dto with { Json = jsonFallback.Json, StatusCode = (int)jsonFallback.StatusCode };
        }

        if (fallback is RedirectFallback redirectFallback)
        {
            dto = dto with { BaseUrl = redirectFallback.BaseUrl };
        }

        await client.SetFallback(dto);
    }
}