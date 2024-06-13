using MachinePark.Core.Models;

namespace MachinePark.UI.Components.Layout.Machines;

public class MachineData
{
    public IEnumerable<MachineModel>? Machines { get; set; }
    public int SetSize { get; set; }
}