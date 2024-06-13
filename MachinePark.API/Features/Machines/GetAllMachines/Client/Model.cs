namespace Machines.GetAllMachines.Client;

public class Request
{
    [QueryParam]
    public int? SetSize { get; set; }
}

public class Response
{
    public IEnumerable<MachineModel> Machines { get; set; }
}