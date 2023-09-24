using FluentValidation;
using System.Security.Cryptography.X509Certificates;

namespace Domain.DTOs.Requests.Validators
{
    public class DeleteUserByIdValidator : AbstractValidator<DeleteUserByIdDTO>
    {
        public DeleteUserByIdValidator()
        {
            RuleFor(x => x.Id).NotEmpty().SetValidator(new GuidValidator());
        }
    }
}
