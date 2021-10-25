using Dovecord.Data;
using Dovecord.Data.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;

namespace Dovecord.Server.Services;

static class ServiceCollectionExtensions
{
    internal static IServiceCollection AddAppAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApi(configuration.GetSection("AzureAdB2C"));

        services.Configure<JwtBearerOptions>(
            JwtBearerDefaults.AuthenticationScheme,
            options =>
            {
                options.TokenValidationParameters.NameClaimType = "name";
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) &&
                            (path.StartsWithSegments("/chathub")))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

        return services;
    }
        
    internal static void RegisterSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "swaggerAADdemo", Version = "v1" });
            c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme {  
                Type = SecuritySchemeType.OAuth2,  
                Flows = new OpenApiOAuthFlows() {  
                    Implicit = new OpenApiOAuthFlow() {  
                        AuthorizationUrl = new Uri("https://login.microsoftonline.com/b8c0dbd8-0eba-4961-b4d6-10b67c5710b6/oauth2/v2.0/authorize"),  
                        TokenUrl = new Uri("https://login.microsoftonline.com/b8c0dbd8-0eba-4961-b4d6-10b67c5710b6/oauth2/v2.0/token"),  
                        Scopes = new Dictionary < string, string > {  
                            {  
                                "API.Access",  
                                "Reads the Weather forecast"  
                            }  
                        }  
                    }  
                }  
            });  
            c.AddSecurityRequirement(new OpenApiSecurityRequirement() {  
                {  
                    new OpenApiSecurityScheme {  
                        Reference = new OpenApiReference {  
                            Type = ReferenceType.SecurityScheme,  
                            Id = "oauth2"  
                        },  
                        Scheme = "oauth2",  
                        Name = "oauth2",  
                        In = ParameterLocation.Header  
                    },  
                    new List < string > ()  
                }  
            }); 
        });
    }
        
    internal static void ConfigureSwagger(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => {  
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "AzureAD_OAuth_API v1");  
            //c.RoutePrefix = string.Empty;    
            c.OAuthClientId("68a431c8-84fd-4fbd-87c6-e3d3ddec67f1");  
            c.OAuthClientSecret("api://89be5e10-1770-45d7-813a-d47242ae2163/API.Access");  
            c.OAuthUseBasicAuthenticationWithAccessCodeGrant();  
        });   
    }
        
    internal static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<IChatService, ChatService>();
        services.AddTransient<IUserService, UserService>();
        return services;
    }

    internal static IServiceCollection AddDatabase(
        this IServiceCollection services)
        => services
            .AddDbContext<ApplicationDbContext>(options => options
                .UseSqlite("Data Source=..\\Data\\DovecordHQ.db"));
}