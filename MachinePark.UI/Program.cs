using MachinePark.UI.Components;
using MachinePark.UI.Extensions;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.RegisterApplicationServices(builder.Configuration);

    var app = builder.Build();

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        app.UseHsts();
    }

    app.UseHttpsRedirection();

    app.UseStaticFiles();
    app.UseAntiforgery();

    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

    app.Run();
    Log.Information("Application exited cleanly.");

}
catch (Exception ex)
 when (ex is not HostAbortedException &&
    ex.Source is not "Microsoft.EntityFrameworkCore.Design")
{

    Log.Fatal("{Message}", new { Message = "Encountered error in boot stage", Error = ex });
}
finally
{
    Log.CloseAndFlush();
}
