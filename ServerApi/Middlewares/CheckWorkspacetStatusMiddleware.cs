using System.Security.Claims;
using System.Text;
using System;

namespace ServerApi.Middlewares;

public class CheckWorkspaceStatusMiddleware
{
    private readonly RequestDelegate next;

    public CheckWorkspaceStatusMiddleware(RequestDelegate next) =>
        this.next=next;

    public async Task Invoke(HttpContext httpContext)
    {
        string urlHost = httpContext.Request.Host.ToString();
        string urlPath = httpContext.Request.Path.ToString();

        if (string.IsNullOrEmpty(urlHost))
        {
            throw new ApplicationException("urlHost must be specified");
        }

        if ((urlPath.ToLower().Contains("/appsettings") == false) && (urlPath.ToLower().Contains("/api/authenticate") == false))
        {
            //TODO: Implement CheckWorkspaceStatus middleware
            //check if user is associated with a workspace
            //return bad request invalid workspaceId
            var workspaces = new Dictionary<string, string>();
            workspaces.Add("2bba57e9-85d8-4f8b-b7d4-a401e96c4170", "myshop");
            workspaces.TryGetValue("2bba57e9-85d8-4f8b-b7d4-a401e96c4179", out string workspaceId);

            if (String.IsNullOrEmpty(workspaceId))
            {
                httpContext.Response.Clear();
                httpContext.Response.Redirect("https://localhost:5001/counter", false);
                //httpContext.Response.StatusCode = (int)StatusCodes.Status400BadRequest;
                //var bytes = Encoding.UTF8.GetBytes("User has no Workspace Assocaited");
                //await httpContext.Response.Body.WriteAsync(bytes);
                return;
            }

            //add workspaceId to the httpContext as an item
            httpContext.Items["workspaceId"] = workspaceId;

            //add any roles to the httpContext claims, if they are used
        }

        await next(httpContext);
    }

}
