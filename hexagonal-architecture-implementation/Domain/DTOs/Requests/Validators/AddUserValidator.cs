using FluentValidation;

namespace Domain.DTOs.Requests.Validators
{
    public class AddUserValidator : AbstractValidator<AddUserDTO>
    {
        public AddUserValidator()
        {
            RuleFor(x => x.Password).NotEmpty().NotNull();
            RuleFor(x => x.Login).NotEmpty().NotNull();
            RuleFor(x => x.Group).NotEmpty().NotNull().IsInEnum();
        }
    }
}
