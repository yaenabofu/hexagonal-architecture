using FluentValidation;

namespace Domain.DTOs.Requests.Validators
{
    public class AddUserValidator : AbstractValidator<AddUserDTO>
    {
        public AddUserValidator()
        {
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.Login).NotEmpty();
            RuleFor(x => x.Group).NotEmpty();
        }
    }
}
