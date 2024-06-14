using MachinePark.UI.EventHanders;
using FastEndpoints;
using MachinePark.Core.Events;
using MachinePark.UI.Components;

namespace MachinePark.UI.Extensions;

public static class WebAppExtensions
{
    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            app.UseHsts();
        }

        app.MapRemote(app.Configuration["API:EventsURI"]!, c =>
        {
            c.Subscribe<MachineDataUpdated, OnMachineDataUpdated>();
        });

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        return app;
    }
}