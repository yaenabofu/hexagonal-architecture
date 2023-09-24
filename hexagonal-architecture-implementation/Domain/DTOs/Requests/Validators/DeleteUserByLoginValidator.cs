using FluentValidation;

namespace Domain.DTOs.Requests.Validators
{
    public class DeleteUserByLoginValidator : AbstractValidator<DeleteUserByLoginDTO>
    {
        public DeleteUserByLoginValidator()
        {
            RuleFor(x => x.Login).NotEmpty().NotNull();
        }
    }
}
