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

    public PhraseService(CantoApiContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PhraseDTO>> GetAllPhrasesAsync()
    {
        var phrases = await _context.Phrases
            .Include(p => p.Theme)
            .ToListAsync();
        return _mapper.Map<IEnumerable<PhraseDTO>>(phrases);
    }

    public async Task<PhraseDTO> GetPhraseByIdAsync(int id)
    {
        var phrase = await _context.Phrases
            .Include(p => p.Theme)
            .FirstOrDefaultAsync(p => p.PhraseId == id);

        if (phrase == null)
            throw new KeyNotFoundException($"Phrase with ID {id} not found.");

        return _mapper.Map<PhraseDTO>(phrase);
    }

    public async Task<IEnumerable<PhraseDTO>> GetPhrasesByThemeAsync(int themeId)
    {
        var phrases = await _context.Phrases
            .Include(p => p.Theme)
            .Where(p => p.ThemeId == themeId)
            .ToListAsync();

        return _mapper.Map<IEnumerable<PhraseDTO>>(phrases);
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