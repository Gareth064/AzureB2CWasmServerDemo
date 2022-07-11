using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServerApi.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WorkspaceController : ControllerBase
{
    private readonly ILogger<WorkspaceController> _logger;

    public WorkspaceController(ILogger<WorkspaceController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Route("{azureUserId}")]
    public IActionResult Get(string azureUserId)
    {
        var workspaces = new Dictionary<string, string>();
        workspaces.Add("2bba57e9-85d8-4f8b-b7d4-a401e96c4179", "myshop");
        workspaces.TryGetValue(azureUserId, out string workspaceId);

        if (String.IsNullOrEmpty(workspaceId))
            return NotFound("User is not associated to a Workspace");

        return Ok(workspaceId);
    }
}
