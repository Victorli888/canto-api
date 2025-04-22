namespace Models.Domain;
public class Theme
{
    public int ThemeId { get; set; }
    public string ThemeName { get; set; }
    public string Description { get; set; }
    
    // Navigation properties
    public ICollection<Phrase> Phrases { get; set; }
}