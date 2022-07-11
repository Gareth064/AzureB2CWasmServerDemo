using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using SharedLibrary.Models;
using System.Net.Http.Json;
using System.Security.Claims;

namespace ClientWasm.Features.Auths;

/// <summary>
/// This Factory can be used to perform extra operations once a user has been authenticated.
/// For example you can add extra "Claims" to their token.
/// </summary>
public class CustomAccountFactory : AccountClaimsPrincipalFactory<CustomRemoteUserAccount>
{
    private readonly ILogger<CustomAccountFactory> logger;
    private readonly IServiceProvider serviceProvider;

    public CustomAccountFactory(
        IAccessTokenProviderAccessor accessor,
        IServiceProvider serviceProvider,
        ILogger<CustomAccountFactory> logger) : base(accessor)
    {
        this.serviceProvider = serviceProvider;
        this.logger = logger;
    }

    public override async ValueTask<ClaimsPrincipal> CreateUserAsync(
        CustomRemoteUserAccount account,
        RemoteAuthenticationUserOptions options)
    {
        var initialUser = await base.CreateUserAsync(account, options);

        if (initialUser.Identity.IsAuthenticated)
        {

            var userAzureId = initialUser.Claims.FirstOrDefault(c => c.Type == "oid").Value;

            HttpClient httpClient = serviceProvider.GetService<IHttpClientFactory>().CreateClient("ServerAPI");
            Workspace workspace = null;
            try
            {
                workspace = await httpClient.GetFromJsonAsync<Workspace>($"Authorization/GetWorkspaceByUserId/{userAzureId}");
            }
            catch
            {
                workspace = null;  
            }

            if (workspace is not null)
            {
                var userIdentity = (ClaimsIdentity)initialUser.Identity;
                userIdentity.AddClaim(new Claim("workspaceId", workspace.Id));
                userIdentity.AddClaim(new Claim("workspaceName", workspace.Name));
                userIdentity.AddClaim(new Claim("workspaceOwnerId", workspace.Owner));
                if (workspace.HasValidSubscription)
                    userIdentity.AddClaim(new Claim("hasValidSubscription", workspace.HasValidSubscription.ToString()));
            }
        }

        return initialUser;
    }
}
