namespace Machines.AddMachine;

public class Mapper : Mapper<Request, Response, Machine>
{
    public override Machine ToEntity(Request r)
    {
        return new()
        {
            Name = r.Name,
            Wattage = r.Wattage,
            Section = r.Section,
            Online = r.Online,
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