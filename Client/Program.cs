using System;
using System.Net.Http;
using Blazored.LocalStorage;
using Dovecord.Client;
using Dovecord.Client.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using Refit;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("Dovecord.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
            
            // TODO: Make a separate service
            // https://dovecord.azurewebsites.net/api for Azure web app
builder.Services.AddRefitClient<IChannelApi>()
    .ConfigureHttpClient(c => { c.BaseAddress = new Uri("https://localhost:5001/api"); })
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
builder.Services.AddRefitClient<IChatApi>()
    .ConfigureHttpClient(c => { c.BaseAddress = new Uri("https://localhost:5001/api"); })
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
builder.Services.AddRefitClient<IUserApi>()
    .ConfigureHttpClient(c => { c.BaseAddress = new Uri("https://localhost:5001/api"); })
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
            

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Dovecord.ServerAPI"));

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);
    options.ProviderOptions.DefaultAccessTokenScopes.Add("https://danovas.onmicrosoft.com/89be5e10-1770-45d7-813a-d47242ae2163/API.Access");
    options.ProviderOptions.LoginMode = "redirect";
});
            
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddMudServices();

await builder.Build().RunAsync();
