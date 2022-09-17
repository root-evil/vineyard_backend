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
        return await vineContext.Prams.AsNoTracking().Include(x => x.Details).ToArrayAsync();
    }

    [HttpGet("/params/{polygonId}")]
    public async Task<Param> GetParams(
        [FromRoute] int polygonId
    )
    {
        var Params = await vineContext.Prams.AsNoTracking()
            .Where(x => x.Polygon.id == polygonId)
            .SingleOrDefaultAsync();

        Params.BetterNearPolygons = await vineContext.Polygons.AsNoTracking()
            .Select(x => new Polygon
            {
                id = x.id,
                scoring = x.scoring,
                area = x.area,
                freeArea = x.freeArea,
                center = x.center,
                width = x.width,
                height = x.height,
                geo = x.geo,
                Param = x.Param,
                regionId = x.regionId,
                Region = x.Region,
            })
            .Where(x => x.scoring > Params.scoring)
            .OrderBy(x => x.scoring)
            .Take(nearCount)
            .ToArrayAsync();
        Params.WorseNearPolygons = await vineContext.Polygons.AsNoTracking()
            .Select(x => new Polygon
            {
                id = x.id,
                scoring = x.scoring,
                area = x.area,
                freeArea = x.freeArea,
                center = x.center,
                width = x.width,
                height = x.height,
                geo = x.geo,
                Param = x.Param,
                regionId = x.regionId,
                Region = x.Region,
            })
            .Where(x => x.scoring < Params.scoring)
            .OrderByDescending(x => x.scoring)
            .Take(nearCount)
            .ToArrayAsync();
        
        return Params;
    }
}
