using FluentValidation;

namespace Domain.DTOs.Requests.Validators
{
    public class DeleteUserByIdValidator : AbstractValidator<DeleteUserByIdDTO>
    {
        public DeleteUserByIdValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
