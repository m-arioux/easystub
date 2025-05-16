using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyStub.UI.Infrastructure;

namespace EasyStub.UI.UseCases.Endpoints;

public class AddEndpointUseCase : IUseCase
{

    public record Input(string Path, HttpMethod Method, HttpStatusCode StatusCode, object Body);

    private readonly EndpointHttpClient client;

    public AddEndpointUseCase(EndpointHttpClient client)
    {
        this.client = client;
    }

    public async Task Handle(Input input)
    {
        var endpointDto = new EndpointDto(input.Path, input.Method.Method, (int)input.StatusCode, input.Body);

        await client.AddEndpointAsync(endpointDto);
    }
}