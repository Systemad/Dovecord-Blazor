using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using Dovecord.Server.Hubs;
using Dovecord.Server.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.OpenApi.Models;

namespace Dovecord.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        const string CorsPolicy = nameof(CorsPolicy);
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddMicrosoftIdentityWebApi(Configuration.GetSection("AzureAdB2C"));
            
            services.AddAppAuthentication(Configuration);
            services.AddCors(options =>
            {
                options.AddPolicy(
                    name: CorsPolicy,
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });
            /*
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
            */
            services.AddControllersWithViews();
            services.AddRazorPages();
            
            services.AddSignalR(options => options.EnableDetailedErrors = true)
                .AddMessagePackProtocol();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
  
                /*
                app.UseSwagger();
                app.UseSwaggerUI(c => {  
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AzureAD_OAuth_API v1");  
                    //c.RoutePrefix = string.Empty;    
                    c.OAuthClientId("68a431c8-84fd-4fbd-87c6-e3d3ddec67f1");  
                    c.OAuthClientSecret("api://89be5e10-1770-45d7-813a-d47242ae2163/API.Access");  
                    c.OAuthUseBasicAuthenticationWithAccessCodeGrant();  
                });   
                */
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors(CorsPolicy);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chathub");
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
