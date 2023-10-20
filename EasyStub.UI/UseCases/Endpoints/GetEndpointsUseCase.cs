using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyStub.UI.Infrastructure;

namespace EasyStub.UI.UseCases.Endpoints;

public class GetEndpointsUseCase : IUseCase
{
    private readonly EndpointHttpClient client;

    public GetEndpointsUseCase(EndpointHttpClient client)
    {
        this.client = client;
    }

    public async Task<List<Endpoint>> Handle()
    {
        var result = await client.GetEndpointsAsync();

        return result.ConvertAll(x =>
            new Endpoint(
                new EndpointId(x.Id),
                x.Path,
                new HttpMethod(x.Method),
                (HttpStatusCode)x.StatusCode));
    }
}