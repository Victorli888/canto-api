namespace Models.DTOs;

public class PhraseDTO
{
    public int PhraseId { get; set; }
    public string ChineseTranslation { get; set; }
    public string EnglishTranslation { get; set; }
    public int ThemeId { get; set; }
    public int ComplexityRating { get; set; }
    public int? RootQuestionId { get; set; }
    public bool IsHidden { get; set; }
    public string ThemeName { get; set; }  // Including theme information
}