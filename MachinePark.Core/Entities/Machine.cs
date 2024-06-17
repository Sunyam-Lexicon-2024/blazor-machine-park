namespace MachinePark.Core.Entities;

public class Machine
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public bool Online { get; set; } = true;
    public double Wattage { get; set; } = 10;
    public int Section { get; set; } = 1;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}