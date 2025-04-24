namespace Models.DTOs;

public class CreatePhraseDTO
{
    public string Cantonese { get; set; }
    public string English { get; set; }
    public int ThemeId { get; set; }
    public int ChallengeRating { get; set; }
    public int? RootQuestionId { get; set; }
    public bool IsHidden { get; set; }
}