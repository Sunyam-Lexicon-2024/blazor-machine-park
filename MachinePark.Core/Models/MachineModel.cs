namespace MachinePark.Core.Models;

public class MachineModel
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public bool Online { get; set; }
    public double Wattage { get; set; }
    public int Section { get; set; }
    public DateTime CreatedAt { get; set; } 
    public DateTime? UpdatedAt { get; set; }
}