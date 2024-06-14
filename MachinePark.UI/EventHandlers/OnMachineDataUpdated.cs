using FastEndpoints;
using MachinePark.Core.Events;
using MachinePark.UI.Services;

namespace MachinePark.UI.EventHanders;

internal class OnMachineDataUpdated(ILogger<OnMachineDataUpdated> logger, INotificationService notificationService) : IEventHandler<MachineDataUpdated>
{
    private readonly ILogger<OnMachineDataUpdated> _logger = logger;
    private readonly INotificationService _notificationService = notificationService;

    public Task HandleAsync(MachineDataUpdated eventModel, CancellationToken ct)
    {
        _logger.LogInformation("{number} - {description}", eventModel.Id, eventModel.Description);
        _notificationService.DispatchNotification();
        return Task.CompletedTask;
    }
}