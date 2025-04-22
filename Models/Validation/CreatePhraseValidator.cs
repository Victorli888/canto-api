using FluentValidation;
using Models.DTOs;

namespace Models.Validation
{
    public class CreatePhraseValidator : AbstractValidator<CreatePhraseDTO>
    {
        public CreatePhraseValidator()
        {
            RuleFor(x => x.ChineseTranslation)
                .NotEmpty()
                .MaximumLength(255);

            RuleFor(x => x.EnglishTranslation)
                .NotEmpty();

            RuleFor(x => x.ThemeId)
                .GreaterThan(0);

            RuleFor(x => x.ComplexityRating)
                .InclusiveBetween(1, 5);
        }
    }
}