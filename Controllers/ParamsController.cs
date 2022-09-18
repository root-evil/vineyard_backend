using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using vineyard_backend.Models;
using vineyard_backend.Context;

namespace vineyard_backend.Controllers;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[Produces(MediaTypeNames.Application.Json)]
public class ParamsController : ControllerBase
{
    const int nearCount = 3;
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
        return await vineContext.Prams.AsNoTracking().ToArrayAsync();
    }

    [HttpGet("/params/{paramId}")]
    public async Task<Param> GetParams(
        [FromRoute] int paramId
    )
    {
        var Params = await vineContext.Prams.AsNoTracking()
            .Where(x => x.id == paramId)
            .SingleOrDefaultAsync();

        Params.BetterNearPolygons = await vineContext.Polygons.AsNoTracking()
            .Where(x => x.scoring > Params.scoring)
            .OrderBy(x => x.scoring)
            .Take(nearCount)
            .ToArrayAsync();
        Params.WorseNearPolygons = await vineContext.Polygons.AsNoTracking()
            .Where(x => x.scoring < Params.scoring)
            .OrderByDescending(x => x.scoring)
            .Take(nearCount)
            .ToArrayAsync();
        
        return Params;
    }
}
