namespace MachinePark.API.Services;

public interface IMachineService
{
    Task<IEnumerable<Machine>> GetAllMachinesAsync();
    Task<Machine?> AddAsync(Machine machine);
    Task SaveChangesAsync();
}
public class MachineService(MachineParkDbContext machineParkDbContext) : IMachineService
{
    private readonly MachineParkDbContext _machineParkDbContext = machineParkDbContext;

    public async Task<IEnumerable<Machine>> GetAllMachinesAsync()
    {
        return await _machineParkDbContext.Machines.ToListAsync();
    }

    public async Task<Machine?> AddAsync(Machine machine)
    {
        var createdMachine = await _machineParkDbContext.Machines.AddAsync(machine);
        return createdMachine.Entity;
    }

    public async Task SaveChangesAsync()
    {
        await _machineParkDbContext.SaveChangesAsync();
    }
}