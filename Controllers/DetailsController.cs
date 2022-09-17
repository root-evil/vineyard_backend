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
public class DetailsController : ControllerBase
{
    private readonly ILogger<DetailsController> logger;
    private readonly VineContext vineContext;

    public DetailsController(ILogger<DetailsController> logger, VineContext vineContext)
    {
        this.logger = logger;
        this.vineContext = vineContext;
    }

    [HttpGet("/details")]
    public async Task<IEnumerable<Detail>> GetDetails()
    {
        return await vineContext.Details.AsNoTracking().ToArrayAsync();
    }

    [HttpGet("/details/{paramId}")]
    public async Task<IEnumerable<Detail>> GetDetails(
        [FromRoute] int paramId
    )
    {
        return await vineContext.Details.AsNoTracking().Where(x => x.Param.id == paramId).ToArrayAsync();
    }
}
