using FluentValidation;
using Models.DTOs;

namespace Models.Validation
{
    public class CreatePhraseValidator : AbstractValidator<CreatePhraseDTO>
    {
        public CreatePhraseValidator()
        {
            RuleFor(x => x.Cantonese)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.English)
                .NotEmpty();

            RuleFor(x => x.ThemeId)
                .GreaterThan(0);

            RuleFor(x => x.ChallengeRating)
                .InclusiveBetween(1, 5);
        }
    }
}