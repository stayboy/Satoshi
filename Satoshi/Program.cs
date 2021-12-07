using Satoshi.Extensions;
using Satoshi.Services;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services; 

services.ConfigureCors();
services.ConfigureIISIntegration();
//services.ConfigureAppSettings(builder.Configuration);

services.ConfigureSqlContext(builder.Configuration); 
services.ConfigureMapster();
 
services.ConfigureRepoServices();
services.ConfigureSwagger();

/*services.Configure<KestrelServerOptions>(o =>
{
    o.Listen(System.Net.IPAddress.Any, 5001, options =>
    {
        options.Protocols = Microsoft.AspNetCore.Server.Kestrel.Core.HttpProtocols.Http1AndHttp2;
        options.UseHttps("./certs/satoshi_grpc.pfx", "wst1install");
    });
});*/ 
var app = builder.Build();

var port = Environment.GetEnvironmentVariable("PORT") ?? "3000";
app.Urls.Add($"https://localhost:{port}");

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json",
                                    $"{builder.Environment.ApplicationName} v1"));
}
if (app.Environment.IsProduction() || app.Environment.IsStaging() || app.Environment.IsEnvironment("staging_2"))
{
    app.UseExceptionHandler();
}

app.UseCors("CorsPolicy");
//app.UseRouting();
app.MapGet("/error", () => Results.Problem("An error occurred.", statusCode: 500))
   .ExcludeFromDescription();

using IServiceScope serviceScope = app.Services.CreateScope();
IServiceProvider provider = serviceScope.ServiceProvider;

var repo = provider.GetRequiredService<IOrderService>();
app.MapProductEndPoint(app.Services, repo);
app.MapOrderEndPoint(app.Services, repo); 

app.MapGet("/", () => "Hello Satoshi!");

await app.RunAsync();
