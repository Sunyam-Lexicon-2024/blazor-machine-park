namespace Machines.GetAllMachines;

public class Mapper : ResponseMapper<MachineModel, Machine>
{
    public override MachineModel FromEntity(Machine m)
    {
        return new()
        {
            Id = m.Id,
            Name = m.Name,
            Online = m.Online,
            Wattage = m.Wattage,
            Section = m.Section,
            CreatedAt = m.CreatedAt,
            UpdatedAt = m.UpdatedAt
        };
    }
}