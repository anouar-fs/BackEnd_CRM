using BackEnd.Dto;
using FluentValidation;

namespace BackEnd.Validation
{
    public class LoginValidation : AbstractValidator<LoginDto>
    {
        public LoginValidation() 
        {
            RuleFor(lg => lg.Username)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(30)
            .Matches("^[a-zA-Z0-9._- ]+$")
            .WithMessage("Username contains invalid characters.");

            RuleFor(lg => lg.password)
                .NotEmpty()
                .MinimumLength(10)
                .MaximumLength(30);
        }
    }
}
