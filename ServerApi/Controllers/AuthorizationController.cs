using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServerApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AuthorizationController : ControllerBase
{
    private readonly ILogger<AuthorizationController> logger;
    private List<Workspace> workspaces;

    public AuthorizationController(ILogger<AuthorizationController> logger)
    {
        this.logger=logger;
        PopulateWorkspaces();
    }

    [HttpGet]
    [Route("{azureUserId}")]
    public Workspace GetWorkspaceByUserId(string azureUserId)
    {
        var workspace = workspaces.Where(w => w.Owner == azureUserId).FirstOrDefault();
        return workspace;
    }

    private void PopulateWorkspaces()
    {
        workspaces = new List<Workspace>();
        workspaces.Add(new Workspace { Id="1234", Name="My Shop", HasValidSubscription=true, Owner="2bba57e9-85d8-4f8b-b7d4-a401e96c4179" });
        workspaces.Add(new Workspace { Id="1234", Name="My Shop", HasValidSubscription=true, Owner="2bba57e9-85d8-4f8b-b7d4-a401e96c4170" });
    }

}
