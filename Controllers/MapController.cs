using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using vineyard_backend.Models;
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
        [FromQuery] string? bounds = null,
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
            queryPolygons = queryPolygons.Where(x => x.Param == null ? false : x.Param.avg_relief_aspect > min_relief_aspect);
            queryMarkers = queryMarkers.Where(x => x.Param == null ? false : x.Param.avg_relief_aspect > min_relief_aspect);
        }
        if(max_relief_aspect != null)
        {
            queryPolygons = queryPolygons.Where(x => x.Param == null ? false : x.Param.avg_relief_aspect < max_relief_aspect);
            queryMarkers = queryMarkers.Where(x => x.Param == null ? false : x.Param.avg_relief_aspect < max_relief_aspect);
        }
        if(min_relief_height != null)
        {
            queryPolygons = queryPolygons.Where(x => x.Param == null ? false : x.Param.avg_relief_height > min_relief_height);
            queryMarkers = queryMarkers.Where(x => x.Param == null ? false : x.Param.avg_relief_height > min_relief_height);
        }
        if(max_relief_height != null)
        {
            queryPolygons = queryPolygons.Where(x => x.Param == null ? false : x.Param.avg_relief_height < max_relief_height);
            queryMarkers = queryMarkers.Where(x => x.Param == null ? false : x.Param.avg_relief_height < max_relief_height);
        }
        if(min_relief_slope != null)
        {
            queryPolygons = queryPolygons.Where(x => x.Param == null ? false : x.Param.avg_relief_slope > min_relief_slope);
            queryMarkers = queryMarkers.Where(x => x.Param == null ? false : x.Param.avg_relief_slope > min_relief_slope);
        }
        if(max_relief_slope != null)
        {
            queryPolygons = queryPolygons.Where(x => x.Param == null ? false : x.Param.avg_relief_slope < max_relief_slope);
            queryMarkers = queryMarkers.Where(x => x.Param == null ? false : x.Param.avg_relief_slope < max_relief_slope);
        }
        if(mix_sunny_days != null)
        {
            queryPolygons = queryPolygons.Where(x => x.Param == null ? false : x.Param.avg_sunny_days > mix_sunny_days);
            queryMarkers = queryMarkers.Where(x => x.Param == null ? false : x.Param.avg_sunny_days > mix_sunny_days);
        }
        if(man_sunny_days != null)
        {
            queryPolygons = queryPolygons.Where(x => x.Param == null ? false : x.Param.avg_sunny_days < man_sunny_days);
            queryMarkers = queryMarkers.Where(x => x.Param == null ? false : x.Param.avg_sunny_days < man_sunny_days);
        }
        if(!string.IsNullOrEmpty(floodedTypes))
        {
            var floodedTypesEnum = floodedTypes.Split(',').Select(x => Enum.Parse<FloodedMonths>(x)).ToHashSet();
            queryPolygons = queryPolygons.Where(x => x.Param == null ? false : floodedTypesEnum.Contains(x.Param.floodedMonths ?? FloodedMonths.No));
            queryMarkers = queryMarkers.Where(x => x.Param == null ? false : floodedTypesEnum.Contains(x.Param.floodedMonths ?? FloodedMonths.No));
        }
        if(!string.IsNullOrEmpty(soilTypes))
        {
            var soilTypesEnum = soilTypes.Split(',').Select(x => Enum.Parse<Soil>(x)).ToHashSet();
            queryPolygons = queryPolygons.Where(x => x.Param == null ? false : x.Param.soil == null ? false : soilTypesEnum.Contains(x.Param.soil.Value));
            queryMarkers = queryMarkers.Where(x => x.Param == null ? false : x.Param.soil == null ? false : soilTypesEnum.Contains(x.Param.soil.Value));
        }

        double[,] boundsRes = null;

        if(!string.IsNullOrEmpty(bounds))
        {
            boundsRes = new double[2,2];
            var boundsArray = bounds.Split(',').Select(x => double.Parse(x)).ToArray();
            if(boundsArray.Count() != 4)
                throw new ArgumentOutOfRangeException(bounds);
            var (xmin, ymin) = (Convert.ToDouble(boundsArray[0]), Convert.ToDouble(boundsArray[1]));
            var (xmax, ymax) = (Convert.ToDouble(boundsArray[2]), Convert.ToDouble(boundsArray[3]));
            (boundsRes[0,0], boundsRes[0,1]) = (xmin, ymin);
            (boundsRes[1,0], boundsRes[1,1]) = (xmax, ymax);
            queryPolygons = queryPolygons
                .Where(x => x.center == null ? false : x.center.Length != 2 ? false :
                    x.center[0] > xmin && x.center[0] < xmax
                    &&
                    x.center[1] > ymin && x.center[1] < ymax);
            queryMarkers = queryMarkers
                .Where(x => x.center == null ? false : x.center.Length != 2 ? false :
                    x.center[0] > xmin && x.center[0] < xmax
                    &&
                    x.center[1] > ymin && x.center[1] < ymax);
        }

        var polygons = await queryPolygons
            .OrderByDescending(x => x.area)
            .Take(limit)
            .ToArrayAsync(cancellationToken);
        var markers = await queryMarkers.ToArrayAsync(cancellationToken);

        
        if(string.IsNullOrEmpty(bounds))
        {
            var allBounds = polygons.Select(x => x.bounds).ToList();
            allBounds.AddRange(markers.Select(x => x.bounds).ToList());
            double xmin = double.MaxValue, ymin = double.MaxValue, xmax = double.MinValue, ymax = double.MinValue;
            foreach(var allBound in allBounds)
            {
                if(allBound == null) continue;
                if(allBound.Length != 4 || allBound.Rank != 2) continue;
                xmin = Math.Min(xmin, allBound[0,0]);
                xmin = Math.Min(xmin, allBound[1,0]);
                ymin = Math.Min(ymin, allBound[0,1]);
                ymin = Math.Min(ymin, allBound[1,1]);
                xmax = Math.Max(xmax, allBound[0,0]);
                xmax = Math.Max(xmax, allBound[1,0]);
                ymax = Math.Max(ymax, allBound[0,1]);
                ymax = Math.Max(ymax, allBound[1,1]);
            }
            if(xmin != double.MinValue 
                && ymin != double.MinValue
                && xmax != double.MaxValue
                && ymax != double.MaxValue)
            {
                boundsRes = new double[2,2];
                (boundsRes[0,0], boundsRes[0,1]) = (xmin, ymin);
                (boundsRes[1,0], boundsRes[1,1]) = (xmax, ymax);
            }
        }

        var allCenters = polygons.Select(x => x.center).ToList();
        allCenters.AddRange(markers.Select(x => x.center).ToList());

        return new MapResponse
        {
            center = allCenters.Any() ? new double[]
            {
                allCenters.Average(x => x == null ? 0 : x.FirstOrDefault()),
                allCenters.Average(x => x == null ? 0 : x.LastOrDefault()),
            } : null,
            bounds = boundsRes,
            Polygons = polygons,
            Markers = markers,
        };
    }
}
