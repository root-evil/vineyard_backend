using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using vineyard_backend.Context;

namespace vineyard_backend.Controllers;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> logger;
    private readonly VineContext vineContext;
    public UsersController(ILogger<UsersController> logger, VineContext vineContext)
    {
        this.logger = logger;
        this.vineContext = vineContext;
    }

    [HttpGet("/users/contact/{polygonId}")]
    public async Task GetContacts(
        [FromRoute] int polygonId,
        CancellationToken cancellationToken
    )
    {
        var polygon = await vineContext.Polygons.Where(x => x.id == polygonId).SingleAsync();
        throw new Exception($"Извините Владелец участка [{polygon.center?.ElementAtOrDefault(0)}, {polygon.center?.ElementAtOrDefault(1)}] ещё не оставил нам своих контактов :(");
    }
}
