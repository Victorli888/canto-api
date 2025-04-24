using Microsoft.AspNetCore.Mvc;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Models.Domain;
using Models.DTOs;
using Services.Interfaces;

namespace Controllers;

    [ApiController]
    [Route("canto-api/v1/[controller]")] //TODO: Does this set the default route for this particular controller?
    public class PhrasesController : ControllerBase
    {
        private readonly IPhraseService _phraseService;
        private readonly ILogger<PhrasesController> _logger;
        private readonly CantoApiContext _context;

        public PhrasesController(CantoApiContext context, ILogger<PhrasesController> logger)
        {
            _logger = logger;
            _context = context;
        }

        //TODO: What async Task<ActionResult>? Non Blocking Task I need an explanation on it and how its commonoly useed in the industry
        // GET -- canto-api/v1/phrases
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhraseDTO>>> GetAll()
        {
            try
            {
                var phrases = await _context.Phrases.ToListAsync();
                return Ok(phrases);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all phrases");
                return StatusCode(500, "Internal server error");
            }
        }

        // GET -- canto-api/v1/phrases/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PhraseDTO>> GetById(int id)
        {
            try
            {
                var phrase = await _context.Phrases.FindAsync(id);
                if (phrase == null)
                {
                    return NotFound();
                }
                return Ok(phrase);
            }

            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error fetching phrase by ID {id}");
                return StatusCode(500, new
                {
                    message = "An error occurred while fetching the phrase",
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                }); 
            }
        }

// PUT: canto-api/v1/phrases/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePhrase(int id, Phrase phrase)
    {
        if (id != phrase.PhraseId)
        {
            return BadRequest();
        }

        _context.Entry(phrase).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PhraseExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    // DELETE: canto-api/v1/phrases/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePhrase(int id)
    {
        var phrase = await _context.Phrases.FindAsync(id);
        if (phrase == null)
        {
            return NotFound();
        }

        _context.Phrases.Remove(phrase);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool PhraseExists(int id)
    {
        return _context.Phrases.Any(e => e.PhraseId == id);
    }
}
