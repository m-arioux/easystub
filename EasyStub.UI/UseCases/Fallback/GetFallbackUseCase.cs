using System.Net;
using EasyStub.UI.Infrastructure;

namespace EasyStub.UI.UseCases.Fallback;

public class GetFallbackUseCase(FallbackHttpClient client, FallbackFactory factory) : IUseCase
{
    private readonly FallbackHttpClient client = client;
    private readonly FallbackFactory factory = factory;

    public async Task<Fallback> Handle()
    {
        var dto = await client.GetFallback();

        return factory.Create(dto);
    }
}

public sealed class UnknownFallbackTypeException(string type) : Exception($"The fallback type {type} is unknown.")
{
}

public class FallbackFactory
{
    public static readonly IReadOnlyList<string> FallbackTypes = [
        NotFoundFallback.Code, RedirectFallback.Code, JsonFallback.Code
    ];

    public Fallback Create(FallbackDto dto) => dto.Type switch
    {
        NotFoundFallback.Code => new NotFoundFallback(),
        RedirectFallback.Code => RedirectFallback.From(dto.BaseUrl),
        JsonFallback.Code => JsonFallback.From(dto.Json, (HttpStatusCode)(dto.StatusCode ?? 0)),
        _ => throw new UnknownFallbackTypeException(dto.Type)
    };
}

public abstract class Fallback(string typeCode)
{
    public string TypeCode { get; } = typeCode;
}

public sealed class NotFoundFallback : Fallback
{
    public const string Code = "NOT_FOUND";

    public NotFoundFallback() : base(Code)
    {

    }
}

public sealed class RedirectFallback : Fallback
{
    public const string Code = "REDIRECT";

    private RedirectFallback(string baseUrl) : base(Code)
    {
        BaseUrl = baseUrl;
    }

    public static RedirectFallback From(string? baseUrl)
    {
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new ArgumentNullException(nameof(baseUrl));
        }

        return new RedirectFallback(baseUrl);
    }

    public string BaseUrl { get; }
}

public sealed class JsonFallback : Fallback
{
    public const string Code = "JSON";

    private JsonFallback(string json, HttpStatusCode statusCode) : base(Code)
    {
        Json = json;
        StatusCode = statusCode;
    }

    public static JsonFallback From(string? json, HttpStatusCode? statusCode)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            throw new ArgumentNullException(nameof(json));
        }

        if (statusCode is null)
        {
            throw new ArgumentNullException(nameof(statusCode));
        }

        return new JsonFallback(json, statusCode.Value);
    }

    public string Json { get; }
    public HttpStatusCode StatusCode { get; }
}