using FluentValidation;

namespace Machines.UpdateMachine;

public class Validator : Validator<Request>
{
    public Validator()
    {
        RuleFor(r => r.Id)
            .NotNull()
            .WithMessage("ID required");

        RuleFor(r => r.Name)
            .NotEmpty()
            .WithMessage("Name required")
            .MinimumLength(5)
            .WithMessage("Minimum name length: 5");

        RuleFor(r => r.Section)
            .NotNull()
            .WithMessage("Section ID required")
            .GreaterThan(0)
            .WithMessage("Section ID cannot be negative number");

        RuleFor(r => r.Online)
            .NotNull()
            .WithMessage("Online status required");

        RuleFor(r => r.Wattage)
            .NotNull()
            .WithMessage("Wattage data required")
            .GreaterThanOrEqualTo(0)
            .WithMessage("Wattage cannot be negative");

        RuleFor(r => r.CreatedAt)
            .NotNull()
            .WithMessage("CreatedAt required")
            .Must(d => d <= DateTime.Now)
            .WithMessage("CreatedAt cannot be in the future");

    }
}