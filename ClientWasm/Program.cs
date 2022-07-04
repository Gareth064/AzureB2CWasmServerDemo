using ClientWasm;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("ServerAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:7016/api/");
})
    .AddHttpMessageHandler(sp =>
    {
        var handler = sp.GetService<AuthorizationMessageHandler>()
        .ConfigureHandler(
            authorizedUrls: new[] { "https://localhost:7016" });
       
        return handler;
    });

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("ServerAPI"));

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);
    options.ProviderOptions.DefaultAccessTokenScopes.Add("https://ecommanageruk.onmicrosoft.com/d1926614-d7e7-45ae-b3e1-50a8cc6c727d/Data.ReadWrite");
});

await builder.Build().RunAsync();
