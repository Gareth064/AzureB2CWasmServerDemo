using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
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

            HttpClient httpClient = serviceProvider.GetService<IHttpClientFactory>().CreateClient("ServerAPI");
            string? tenantId = await httpClient.GetStringAsync("tenant/2bba57e9-85d8-4f8b-b7d4-a401e96c4179");

            if (String.IsNullOrWhiteSpace(tenantId) == false)
            {
                var userIdentity = (ClaimsIdentity)initialUser.Identity;
                userIdentity.AddClaim(new Claim("tenantId", tenantId));
                userIdentity.AddClaim(new Claim("HasValidSubscription", "true"));
            }
        }

        return initialUser;
    }
}
