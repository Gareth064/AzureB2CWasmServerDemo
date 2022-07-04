using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServerApi.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TenantController : ControllerBase
{
    private readonly ILogger<TenantController> _logger;

    public TenantController(ILogger<TenantController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Route("{azureUserId}")]
    public IActionResult Get(string azureUserId)
    {
        var tenants = new Dictionary<string, string>();
        tenants.Add("2bba57e9-85d8-4f8b-b7d4-a401e96c4179", "myshop");
        tenants.TryGetValue(azureUserId, out string tenantId);

        if (String.IsNullOrEmpty(tenantId))
            return NotFound("User has no tenant assigned to them");

        return Ok(tenantId);
    }
}
