namespace Models.DTOs;

public class PhraseDTO
{
    public int PhraseId { get; set; }
    public string Cantonese { get; set; }
    public string English { get; set; }
    public int ThemeId { get; set; }
    public int ChallengeRating { get; set; }
    public int? RootQuestionId { get; set; }
    public bool IsHidden { get; set; }
    public string ThemeName { get; set; }  // Including theme information
}