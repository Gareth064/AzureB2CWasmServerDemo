using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace ClientWasm.Features.Auths;
/// <summary>
/// This class allows us to extend an authenticated users properties to include our own custom ones.
/// </summary>
public class CustomRemoteUserAccount : RemoteUserAccount
{
    public string TenantId { get; set; }
    public bool HasValidSubscription { get; set; } = false;
}
