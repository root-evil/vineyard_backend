using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

    [HttpGet("/map")]
    public async Task<MapResponse> GetPolygons(
        CancellationToken cancellationToken,
        [FromQuery] int? regionId = null,
        [FromQuery] double? min_relief_aspect = null,
        [FromQuery] double? max_relief_aspect = null,
        [FromQuery] double? min_relief_height = null,
        [FromQuery] double? max_relief_height = null,
        [FromQuery] double? min_relief_slope = null,
        [FromQuery] double? max_relief_slope = null,
        [FromQuery] double? mix_sunny_days = null,
        [FromQuery] double? man_sunny_days = null
    )
    {
        var queryPolygons = vineContext.Polygons.AsNoTracking();

        if(regionId != null)
            queryPolygons = queryPolygons.Where(x => x.regionId == regionId);
        if(min_relief_aspect != null)
            queryPolygons = queryPolygons.Where(x => x.Param.avg_relief_aspect > min_relief_aspect);
        if(max_relief_aspect != null)
            queryPolygons = queryPolygons.Where(x => x.Param.avg_relief_aspect < max_relief_aspect);
        if(min_relief_height != null)
            queryPolygons = queryPolygons.Where(x => x.Param.avg_relief_height > min_relief_height);
        if(max_relief_height != null)
            queryPolygons = queryPolygons.Where(x => x.Param.avg_relief_height < max_relief_height);
        if(min_relief_slope != null)
            queryPolygons = queryPolygons.Where(x => x.Param.avg_relief_slope > min_relief_slope);
        if(max_relief_slope != null)
            queryPolygons = queryPolygons.Where(x => x.Param.avg_relief_slope < max_relief_slope);
        if(mix_sunny_days != null)
            queryPolygons = queryPolygons.Where(x => x.Param.avg_sunny_days > mix_sunny_days);
        if(man_sunny_days != null)
            queryPolygons = queryPolygons.Where(x => x.Param.avg_sunny_days < man_sunny_days);

        var queryMarkers = vineContext.Markers.AsNoTracking();

        if(regionId != null)
            queryMarkers = queryMarkers.Where(x => x.regionId == regionId);
        if(min_relief_aspect != null)
            queryMarkers = queryMarkers.Where(x => x.Param.avg_relief_aspect > min_relief_aspect);
        if(max_relief_aspect != null)
            queryMarkers = queryMarkers.Where(x => x.Param.avg_relief_aspect < max_relief_aspect);
        if(min_relief_height != null)
            queryMarkers = queryMarkers.Where(x => x.Param.avg_relief_height > min_relief_height);
        if(max_relief_height != null)
            queryMarkers = queryMarkers.Where(x => x.Param.avg_relief_height < max_relief_height);
        if(min_relief_slope != null)
            queryMarkers = queryMarkers.Where(x => x.Param.avg_relief_slope > min_relief_slope);
        if(max_relief_slope != null)
            queryMarkers = queryMarkers.Where(x => x.Param.avg_relief_slope < max_relief_slope);
        if(mix_sunny_days != null)
            queryMarkers = queryMarkers.Where(x => x.Param.avg_sunny_days > mix_sunny_days);
        if(man_sunny_days != null)
            queryMarkers = queryMarkers.Where(x => x.Param.avg_sunny_days < man_sunny_days);

        var polygons = await queryPolygons.ToArrayAsync(cancellationToken);
        var markers = await queryMarkers.ToArrayAsync(cancellationToken);

        var allCenters = polygons.Select(x => x.center).ToList();
        allCenters.AddRange(markers.Select(x => x.center).ToList());

        return new MapResponse
        {
            center = allCenters.Any() ? new double[]
            {
                allCenters.Average(x => x.FirstOrDefault()),
                allCenters.Average(x => x.LastOrDefault()),
            } : null,
            Polygons = polygons,
            Markers = markers,
        };
    }
}