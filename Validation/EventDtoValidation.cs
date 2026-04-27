
using FluentValidation;

public class EventDtoValidation : AbstractValidator<EventDto>
{
    public EventDtoValidation()
    {
        RuleFor(ev => ev.Date > DateOnly.FromDateTime(DateTime.Today))
            .NotEmpty()
            .WithMessage("Invalid Date please enter a valid date");
        RuleFor(ev => ev.Lead)
            .NotEmpty();
        //RuleFor(ev => ev.advisor)
        //    .NotEmpty();
        RuleFor(ev => ev.HeureDebut)
            .Must(BeAllowedHour)
            .WithMessage("Allowed hours are: 09:00, 10:00, 11:00, 14:00, 15:00, 16:00, 17:00");
        RuleFor(ev => ev.Statut)
            .IsInEnum()
            .WithMessage("Invalid meeting status.");

    }

    private bool BeAllowedHour(TimeOnly time)
    {
        var allowedTime = new TimeOnly[] {
            new TimeOnly(9, 0),
            new TimeOnly(10, 0),
            new TimeOnly(11, 0),
            new TimeOnly(14, 0),
            new TimeOnly(15, 0),
            new TimeOnly(16, 0),
            new TimeOnly(17, 0)
        };
        return allowedTime.Contains(time);
    }
}

