namespace MachinePark.Core.Models;

public class MachineModel
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public bool Active { get; set; }
    public int Section { get; set; }
}