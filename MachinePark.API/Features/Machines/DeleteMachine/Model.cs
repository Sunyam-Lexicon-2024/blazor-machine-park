namespace Machines.DeleteMachine;

public class Request {
    public Guid MachineId { get; set; }
}

public class Response
{
    public Guid MachineId {get; set;}
}