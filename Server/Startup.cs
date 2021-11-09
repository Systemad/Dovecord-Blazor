using Dovecord.Server.Hubs;
using Dovecord.Server.Services;

var builder = WebApplication.CreateBuilder(args);
const string CorsPolicy = nameof(CorsPolicy);

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddApplicationServices();
builder.Services.AddAppAuthentication(builder.Configuration);
builder.Services.AddCors(options =>
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
builder.Services.RegisterSwagger();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
            
builder.Services.AddSignalR(options => options.EnableDetailedErrors = true)
    .AddMessagePackProtocol();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
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

app.MapRazorPages();
app.MapControllers();
app.MapHub<ChatHub>("/chathub");
app.MapFallbackToFile("index.html");

app.Run();
