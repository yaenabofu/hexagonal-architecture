using FluentValidation;

namespace Domain.DTOs.Requests.Validators
{
    public class GetUserByIdValidator : AbstractValidator<GetUserByIdDTO>
    {
        public GetUserByIdValidator()
        {
            RuleFor(x => x.Id).NotEmpty().SetValidator(new GuidValidator());
        }
    }
}
