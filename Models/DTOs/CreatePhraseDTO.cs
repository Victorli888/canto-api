namespace Models.DTOs;

public class CreatePhraseDTO
{
    public string ChineseTranslation { get; set; }
    public string EnglishTranslation { get; set; }
    public int ThemeId { get; set; }
    public int ComplexityRating { get; set; }
    public int? RootQuestionId { get; set; }
    public bool IsHidden { get; set; }
}