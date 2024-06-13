namespace Machines.GetAllMachines.Server;

public class Request
{
    [QueryParam]
    public string? Search { get; set; }
    [QueryParam]
    public int? Page { get; set; }
    [QueryParam]
    public int? PageSize { get; set; }
    [QueryParam]
    public int? SetSize { get; set; }
    [QueryParam]
    public string? SortBy { get; set; }
    [QueryParam]
    public string? SortDirection { get; set; }
}

public class Response
{
    public IEnumerable<MachineModel> Machines { get; set; }
    public int SetSize { get; set; }
}