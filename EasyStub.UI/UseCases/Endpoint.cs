namespace EasyStub.UI.UseCases;

using System.Net;

public record Endpoint(EndpointId Id, string Path, HttpMethod Method, HttpStatusCode StatusCode);

public record EndpointId(int Value);