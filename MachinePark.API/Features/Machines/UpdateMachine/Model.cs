namespace Machines.UpdateMachine;


public class Request
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool Online { get; set; }
    public double Wattage { get; set; }
    public int Section { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt => DateTime.UtcNow;
}

public class Response
{
    public int Id { get; set; }
}