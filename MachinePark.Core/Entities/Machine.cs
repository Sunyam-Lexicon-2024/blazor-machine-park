namespace MachinePark.Core.Entities;

public class Machine
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public bool Online { get; set; }
    public double Wattage { get; set; }
    public int Section { get; set; }
}