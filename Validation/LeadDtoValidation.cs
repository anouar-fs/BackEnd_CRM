using BackEnd.Dto;
using FluentValidation;

namespace BackEnd.Validation;
public class LeadDtoValidation : AbstractValidator<LeadDto>
{
    public LeadDtoValidation() 
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(30);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(30);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Phone)
            .NotEmpty()
            .Matches(@"^\+?[0-9]{8,15}$")
            .WithMessage("Invalid phone number");

        RuleFor(x => x.Source)
            .NotEmpty();

        RuleFor(x => x.ProductInterest)
            .MaximumLength(100);

        RuleFor(x => x.UtmCampaign)
            .MaximumLength(100);
    }  
}

