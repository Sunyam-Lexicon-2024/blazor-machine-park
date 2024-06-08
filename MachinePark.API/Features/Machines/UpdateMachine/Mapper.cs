namespace Machines.UpdateMachine;

public class Mapper : Mapper<Request, Response, Machine>
{
    public override Machine ToEntity(Request r)
    {
        return new()
        {
            Id = r.Id,
            Name = r.Name,
            Wattage = r.Wattage,
            Section = r.Section,
            Online = r.Online,
            CreatedAt = r.CreatedAt,
            UpdatedAt = r.UpdatedAt,
        };
    }

    public override Response FromEntity(Machine m)
    {
        return new()
        {
            Id = m.Id
        };
    }
}