namespace MachinePark.API.Services;

public interface IMachineService
{
    Task<IEnumerable<Machine>> GetAllMachinesAsync();
}
public class MachineService(MachineParkDbContext machineParkDbContext) : IMachineService
{
    private readonly MachineParkDbContext _machineParkDbContext = machineParkDbContext;

    public async Task<IEnumerable<Machine>> GetAllMachinesAsync()
    {
        return await _machineParkDbContext.Machines.ToListAsync();
    }
}