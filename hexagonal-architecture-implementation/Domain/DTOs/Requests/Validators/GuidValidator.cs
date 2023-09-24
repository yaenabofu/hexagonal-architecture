using FluentValidation;

namespace Domain.DTOs.Requests.Validators
{
    public class GuidValidator : AbstractValidator<Guid>
    {
        public bool Validate(string input)
        {
            return Guid.TryParse(input, out _);
        }
    }
}
