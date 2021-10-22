using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Dovecord.Data;
using Dovecord.Server.Hubs;
using Dovecord.Server.Services;
using Microsoft.EntityFrameworkCore;

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
        
        public void ConfigureServices(IServiceCollection services)
        {
            // Data Source=DovecordHQ.db for Azure web app
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite("Data Source=..\\Dovecord.Data\\DovecordHQ.db")
            );
            services.AddApplicationServices();
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
            services.AddRouting(options => options.LowercaseUrls = true);
            services.RegisterSwagger();
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
                app.ConfigureSwagger();
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
