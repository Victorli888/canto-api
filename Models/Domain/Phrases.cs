namespace Models.Domain;

public class Phrase
{
    public int PhraseId { get; set; }
    public string Cantonese { get; set; }
    public string English { get; set; }
    public int ThemeId { get; set; }
    public string ChallengeRating { get; set; }
    public int? RootQuestionId { get; set; }
    public bool isHidden { get; set; }

    // Navigation properties
    public Theme Theme { get; set; }
}
