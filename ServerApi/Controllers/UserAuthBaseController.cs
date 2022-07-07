using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Models;

namespace ServerApi.Controllers;

public class UserAuthBaseController : ControllerBase
{
    public bool Tenant
    {
        get
        {
            object multiTenant;

            if (!HttpContext.User.HasClaim(c => c.Type == "tenantId"))
                throw new ApplicationException("TenantId missing from context");
            

            //var tenant = tenantService.RetrieveTenantByIdAsync(multiTenant)



            return true;
        }
    }
}
