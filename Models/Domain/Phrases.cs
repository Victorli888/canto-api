namespace Models.Domain;

public class Phrase
{
    public int PhraseId { get; set; }
    public string Cantonese { get; set; }
    public string English { get; set; }
    public int ThemeId { get; set; }
    public int ChallengeRating { get; set; }
    public int? RootQuestionId { get; set; }
    public bool IsHidden { get; set; }

    // Navigation properties
    public Theme Theme { get; set; }
}
