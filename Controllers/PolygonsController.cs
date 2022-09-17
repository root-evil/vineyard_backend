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
public class PolygonsController : ControllerBase
{
    private readonly ILogger<PolygonsController> logger;
    private readonly VineContext vineContext;

    public PolygonsController(ILogger<PolygonsController> logger, VineContext vineContext)
    {
        this.logger = logger;
        this.vineContext = vineContext;
    }

    [HttpGet("/polygons")]
    public async Task<MapResponse<IEnumerable<Polygon>>> GetPolygons()
    {
        var polygons = await vineContext.Polygons.AsNoTracking().Include(x => x.Region).ToArrayAsync();
        var center = new double[2];
        center[0] = 0;
        center[1] = 0;
        foreach(var polygon in polygons)
        {
            center[0] += polygon.center[0];
            center[1] += polygon.center[1];
        }

        center[0] /= polygons.Length;
        center[1] /= polygons.Length;
        return new MapResponse<IEnumerable<Polygon>>
        {
            center = center,
            data = polygons,
        };
    }

    [HttpGet("/polygons/{regionId}")]
    public async Task<MapResponse<IEnumerable<Polygon>>> GetPolygons(
        [FromRoute] int regionId
    )
    {
        var polygons = await vineContext.Polygons.AsNoTracking().Where(x => x.Region.id == regionId).ToArrayAsync();
        var center = new double[2];
        center[0] = 0;
        center[1] = 0;
        foreach(var polygon in polygons)
        {
            center[0] += polygon.center[0];
            center[1] += polygon.center[1];
        }

        center[0] /= polygons.Length;
        center[1] /= polygons.Length;
        return new MapResponse<IEnumerable<Polygon>>
        {
            center = center,
            data = polygons,
        };
    }
}
