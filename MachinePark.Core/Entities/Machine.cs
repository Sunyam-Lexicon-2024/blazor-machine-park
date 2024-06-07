namespace MachinePark.Core.Entities;

public class Machine
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public bool Active { get; set; }
    public int Section { get; set; }
}