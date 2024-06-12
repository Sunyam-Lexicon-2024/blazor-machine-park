namespace Machines.GetAllMachines.Paginated;

public class PaginatedRequest
{
    [QueryParam]
    public int Page { get; set; }
    [QueryParam]
    public int PageSize { get; set; }
}