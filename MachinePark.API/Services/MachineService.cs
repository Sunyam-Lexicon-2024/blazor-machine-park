using System.Linq.Expressions;

namespace MachinePark.API.Services;

public interface IMachineService
{
    Task<IEnumerable<Machine>> GetAllMachinesAsync();
    Task<Machine?> GetByIdAsync(int machineId);
    Task<Machine?> AddAsync(Machine machine);
    Task<Machine?> UpdateAsync(Machine machine);
    Task<Machine?> DeleteAsync(int machineId);

    Task<bool> AnyAsync(Expression<Func<Machine, bool>> expression);
    Task SaveChangesAsync();
}
public class MachineService(MachineParkDbContext machineParkDbContext) : IMachineService
{
    private readonly MachineParkDbContext _machineParkDbContext = machineParkDbContext;

    public async Task<IEnumerable<Machine>> GetAllMachinesAsync()
    {
        return await _machineParkDbContext.Machines.ToListAsync();
    }

    public async Task<Machine?> GetByIdAsync(int machineId)
    {
        return await _machineParkDbContext.Machines.FirstOrDefaultAsync(m => m.Id == machineId);
    }

    public async Task<Machine?> AddAsync(Machine machine)
    {
        var createdMachine = (await _machineParkDbContext.Machines.AddAsync(machine)).Entity;
        return createdMachine;
    }
    public async Task<Machine?> UpdateAsync(Machine machine)
    {
        var updatedMachine = await Task.Run(() => _machineParkDbContext.Machines.Update(machine).Entity);
        return updatedMachine;
    }

    public async Task<Machine?> DeleteAsync(int machineId)
    {
        var machineToDelete = await GetById(machineId);
        if (machineToDelete is null)
        {
            return null;
        }
        else
        {
            var deletedMachine = _machineParkDbContext.Machines.Remove(machineToDelete);
            return deletedMachine.Entity;
        }
    }

    public async Task<bool> AnyAsync(Expression<Func<Machine, bool>> expression)
    {
        return await _machineParkDbContext.Machines.AnyAsync(expression);
    }

    public async Task SaveChangesAsync()
    {
        await _machineParkDbContext.SaveChangesAsync();
    }
}