using FluentValidation;

namespace Domain.DTOs.Requests.Validators
{
    public class GetUserByLoginValidator : AbstractValidator<GetUserByLoginDTO>
    {
        public GetUserByLoginValidator()
        {
            RuleFor(x => x.Login).NotEmpty().NotNull();
        }
    }
}
