using AutoMapper;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Services.Interfaces;

public class PhraseService : IPhraseService
//TODO: is it necessary to implement the interface here? Does it cause too much overhead? Why can't we directly define the PhraseService class?
//TODO: What exactly is PhraseService Class for?
{
    private readonly CantoApiContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<PhraseService> _logger;

    public PhraseService(CantoApiContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PhraseDTO>> GetAllPhrasesAsync()
    {
        var phrases = await _context.Phrases
            .Select(p => new PhraseDTO
            {
                PhraseId = p.PhraseId,
                Cantonese = p.Cantonese,
                English = p.English,
                ThemeId = p.ThemeId,
                RootQuestionId = p.RootQuestionId,
                ChallengeRating = p.ChallengeRating,
                IsHidden = p.IsHidden
            })
            .ToListAsync();
        return phrases;
    }

    public async Task<PhraseDTO> GetPhraseByIdAsync(int id)
    {
        try
        {
            var phrase = await _context.Phrases
            .Include(p => p.Theme)
            .FirstOrDefaultAsync(p => p.PhraseId == id);

            if (phrase == null)
            {
                throw new KeyNotFoundException($"Phrase with ID {id} not found.");
            }

            _logger.LogInformation($"SQL Query: {_context.Phrases.Where(p => p.PhraseId == id).ToQueryString()}");

            return _mapper.Map<PhraseDTO>(phrase);
        }
        
        catch
        {
            _logger.LogError($"Error fetching phrase with ID {id}");
            throw;
        }

    }

    public async Task<IEnumerable<PhraseDTO>> GetPhrasesByThemeAsync(int themeId)
    {
        throw new NotImplementedException();
    }

    public Task<PhraseDTO> CreatePhraseAsync(CreatePhraseDTO createPhraseDto)
    {
        throw new NotImplementedException();
    }

    public Task<PhraseDTO> UpdatePhraseAsync(int id, CreatePhraseDTO updatePhraseDto)
    {
        throw new NotImplementedException();
    }

    public Task DeletePhraseAsync(int id)
    {
        throw new NotImplementedException();
    }
}