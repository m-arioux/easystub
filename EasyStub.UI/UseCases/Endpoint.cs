namespace EasyStub.UI.UseCases;

using System.Net;

public record Endpoint(string Path, HttpMethod Method, HttpStatusCode StatusCode);