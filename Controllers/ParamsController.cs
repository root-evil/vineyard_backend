using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using vineyard_backend.Models;
using vineyard_backend.Services;
using vineyard_backend.Context;

namespace vineyard_backend.Controllers;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class ParamsController : ControllerBase
{
    private readonly ILogger<ParamsController> logger;
    private readonly VineContext vineContext;

    public ParamsController(ILogger<ParamsController> logger, VineContext vineContext)
    {
        this.logger = logger;
        this.vineContext = vineContext;
    }

    [HttpGet("/params")]
    public async Task<IEnumerable<Param>> GetParams()
    {
        return await vineContext.Prams.AsNoTracking().Include(x => x.Details).ToArrayAsync();
    }

    [HttpGet("/params/{polygonId}")]
    public async Task<IEnumerable<Param>> GetParams(
        [FromRoute] int polygonId
    )
    {
        return await vineContext.Prams.AsNoTracking().Where(x => x.Polygon.id == polygonId).ToArrayAsync();
    }
}
