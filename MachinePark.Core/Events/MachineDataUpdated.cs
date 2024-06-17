using FastEndpoints;

namespace MachinePark.Core.Events;

public class MachineDataUpdated : IEvent
{
    public int? Id { get; set; }
    public string? Description { get; set; }
}