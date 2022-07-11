namespace ServerApi.Middlewares;

public class CheckSubscriptionStatusMiddleware
{
    private readonly RequestDelegate next;

    public CheckSubscriptionStatusMiddleware(RequestDelegate next) =>
        this.next=next;

    public async Task Invoke(HttpContext httpContext)
    {
        var urlPath = httpContext.Request.Path.ToString();

        if ((urlPath.ToLower().Contains("/appsettings/subscription") == false) && (urlPath.ToLower().Contains("/api/authenticate") == false))
        {
            //TODO: Implement Subscription status checking middleware
            var hasValidSubscription = true;

            if (hasValidSubscription == false)
            {
                httpContext.Response.Clear();
                httpContext.Response.StatusCode = (int)StatusCodes.Status400BadRequest;
                return;
            }
        }

        await next(httpContext);
    }
}
