using FastEndpoints;
using MachinePark.Core.Events;

namespace MachinePark.UI.EventHanders;

internal class OnMachineDataUpdated(ILogger<OnMachineDataUpdated> logger) : IEventHandler<MachineDataUpdated>
{
    private readonly ILogger<OnMachineDataUpdated> _logger = logger;

    public Task HandleAsync(MachineDataUpdated eventModel, CancellationToken ct)
    {
        _logger.LogInformation("{number} - {description}", eventModel.Id, eventModel.Description);
        return Task.CompletedTask;
    }
}