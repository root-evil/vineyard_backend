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
public class RegionsController : ControllerBase
{
    private readonly ILogger<RegionsController> logger;
    private readonly VineContext vineContext;

    public RegionsController(ILogger<RegionsController> logger, VineContext vineContext)
    {
        this.logger = logger;
        this.vineContext = vineContext;
    }

    [HttpGet("/regions")]
    public async Task<IEnumerable<Region>> GetRegions()
    {
        return await vineContext.Regions.AsNoTracking().ToArrayAsync();
    }
}
