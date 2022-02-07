using System;
using System.Net.Http;
using Blazored.LocalStorage;
using Dovecord.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using Refit;

namespace Dovecord.Client.Extensions;

public static class WasmHostExtension
{
    public static void AddClientServices(this WebAssemblyHostBuilder builder)
    {
        builder.Services.AddHttpClient("Dovecord.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
            .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
        builder.Services.AddRefitClient<IChannelApi>()
            .ConfigureHttpClient(c => { c.BaseAddress = new Uri("https://localhost:7045/api/channels"); })
            .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
        builder.Services.AddRefitClient<IMessageApi>()
            .ConfigureHttpClient(c => { c.BaseAddress = new Uri("https://localhost:7045/api/messages"); })
            .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
        builder.Services.AddRefitClient<IUserApi>()
            .ConfigureHttpClient(c => { c.BaseAddress = new Uri("https://localhost:7045/api"); })
            .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

        builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Dovecord.ServerAPI"));

        builder.Services.AddMsalAuthentication(options =>
        {
            builder.Configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);
            options.ProviderOptions.DefaultAccessTokenScopes.Add("https://danovas.onmicrosoft.com/89be5e10-1770-45d7-813a-d47242ae2163/API.Access");
            options.ProviderOptions.LoginMode = "redirect";
        });
            
        builder.Services.AddBlazoredLocalStorage();
        builder.Services.AddMudServices();
    }
}