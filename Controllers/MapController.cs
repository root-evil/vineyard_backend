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
public class MapController : ControllerBase
{
    private readonly ILogger<MapController> logger;
    private readonly VineContext vineContext;

    public MapController(ILogger<MapController> logger, VineContext vineContext)
    {
        this.logger = logger;
        this.vineContext = vineContext;
    }

    [HttpGet("/regions")]
    public async Task<IEnumerable<Region>> GetPolygons(
        CancellationToken cancellationToken
    )
    {
        return await vineContext.Regions.AsNoTracking().ToArrayAsync(cancellationToken);
    }

    /// <param name="floodedTypes">flooded types, comma separated : No, From0To1, From1To3, From3To6</param>
    /// <param name="soilTypes">soil types, comma separated : Clay, SiltyClay, SlityClayLoam, SandyClay, SandyClayLoam, ClayLoam, Silt, SiltLoam, Loam, Sand, LoamySand, SandyLoam</param>
    [HttpGet("/map")]
    public async Task<MapResponse> GetPolygons(
        CancellationToken cancellationToken,
        [FromQuery] int? regionId = null,
        [FromQuery] int limit = 100,
        [FromQuery] long? minArea = null,
        [FromQuery] long? maxArea = null,
        [FromQuery] long? minFreeArea = null,
        [FromQuery] long? maxFreeArea = null,
        [FromQuery] double? min_relief_aspect = null,
        [FromQuery] double? max_relief_aspect = null,
        [FromQuery] double? min_relief_height = null,
        [FromQuery] double? max_relief_height = null,
        [FromQuery] double? min_relief_slope = null,
        [FromQuery] double? max_relief_slope = null,
        [FromQuery] double? mix_sunny_days = null,
        [FromQuery] double? man_sunny_days = null,
        [FromQuery] string? floodedTypes = null,
        [FromQuery] string? soilTypes = null
    )
    {
        var queryPolygons = vineContext.Polygons.AsNoTracking();
        var queryMarkers = vineContext.Markers.AsNoTracking();

        if(minArea != null)
            queryPolygons = queryPolygons.Where(x => x.area > minArea);
        if(maxArea != null)
            queryPolygons = queryPolygons.Where(x => x.area < maxArea);
        if(minFreeArea != null)
            queryPolygons = queryPolygons.Where(x => x.freeArea > minFreeArea);
        if(maxFreeArea != null)
            queryPolygons = queryPolygons.Where(x => x.freeArea < maxFreeArea);
        if(regionId != null)
        {
            queryPolygons = queryPolygons.Where(x => x.regionId == regionId);
            queryMarkers = queryMarkers.Where(x => x.regionId == regionId);
        }
        if(min_relief_aspect != null)
        {
            queryPolygons = queryPolygons.Where(x => x.Param.avg_relief_aspect > min_relief_aspect);
            queryMarkers = queryMarkers.Where(x => x.Param.avg_relief_aspect > min_relief_aspect);
        }
        if(max_relief_aspect != null)
        {
            queryPolygons = queryPolygons.Where(x => x.Param.avg_relief_aspect < max_relief_aspect);
            queryMarkers = queryMarkers.Where(x => x.Param.avg_relief_aspect < max_relief_aspect);
        }
        if(min_relief_height != null)
        {
            queryPolygons = queryPolygons.Where(x => x.Param.avg_relief_height > min_relief_height);
            queryMarkers = queryMarkers.Where(x => x.Param.avg_relief_height > min_relief_height);
        }
        if(max_relief_height != null)
        {
            queryPolygons = queryPolygons.Where(x => x.Param.avg_relief_height < max_relief_height);
            queryMarkers = queryMarkers.Where(x => x.Param.avg_relief_height < max_relief_height);
        }
        if(min_relief_slope != null)
        {
            queryPolygons = queryPolygons.Where(x => x.Param.avg_relief_slope > min_relief_slope);
            queryMarkers = queryMarkers.Where(x => x.Param.avg_relief_slope > min_relief_slope);
        }
        if(max_relief_slope != null)
        {
            queryPolygons = queryPolygons.Where(x => x.Param.avg_relief_slope < max_relief_slope);
            queryMarkers = queryMarkers.Where(x => x.Param.avg_relief_slope < max_relief_slope);
        }
        if(mix_sunny_days != null)
        {
            queryPolygons = queryPolygons.Where(x => x.Param.avg_sunny_days > mix_sunny_days);
            queryMarkers = queryMarkers.Where(x => x.Param.avg_sunny_days > mix_sunny_days);
        }
        if(man_sunny_days != null)
        {
            queryPolygons = queryPolygons.Where(x => x.Param.avg_sunny_days < man_sunny_days);
            queryMarkers = queryMarkers.Where(x => x.Param.avg_sunny_days < man_sunny_days);
        }
        if(!string.IsNullOrEmpty(floodedTypes))
        {
            var floodedTypesEnum = floodedTypes.Split(',').Select(x => Enum.Parse<FloodedMonths>(x)).ToHashSet();
            queryPolygons = queryPolygons.Where(x => floodedTypesEnum.Contains(x.Param.floodedMonths ?? FloodedMonths.No));
            queryMarkers = queryMarkers.Where(x => floodedTypesEnum.Contains(x.Param.floodedMonths ?? FloodedMonths.No));
        }
        if(!string.IsNullOrEmpty(soilTypes))
        {
            var soilTypesEnum = soilTypes.Split(',').Select(x => Enum.Parse<Soil>(x)).ToHashSet();
            queryPolygons = queryPolygons.Where(x => soilTypesEnum.Contains(x.Param.soil.Value));
            queryMarkers = queryMarkers.Where(x => soilTypesEnum.Contains(x.Param.soil.Value));
        }


        var polygons = await queryPolygons
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
            .OrderByDescending(x => x.area)
            .Take(limit)
            .ToArrayAsync(cancellationToken);
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
