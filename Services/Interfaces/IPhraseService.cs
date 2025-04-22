using Models.DTOs;

namespace Services.Interfaces;
public interface IPhraseService
{
    Task<IEnumerable<PhraseDTO>> GetAllPhrasesAsync();
    Task<PhraseDTO> GetPhraseByIdAsync(int id);
    Task<IEnumerable<PhraseDTO>> GetPhrasesByThemeAsync(int themeId);
    Task<PhraseDTO> CreatePhraseAsync(CreatePhraseDTO createPhraseDto);
    Task<PhraseDTO> UpdatePhraseAsync(int id, CreatePhraseDTO updatePhraseDto);
    Task DeletePhraseAsync(int id);
}