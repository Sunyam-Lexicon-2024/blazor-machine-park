namespace Machines.AddMachine;


public class Request
{
    public string Name { get; set; }
    public bool Online { get; set; }
    public double Wattage { get; set; }
    public int Section { get; set; }
}

public class Response
{
    public Guid Id { get; set; }
}