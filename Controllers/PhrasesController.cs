using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.DTOs;
using Services.Interfaces;

namespace Controllers
{
    [ApiController]
    [Route("canto-api/v1/[controller]")] //TODO: Does this set the default route for this particular controller?
    public class PhrasesController : ControllerBase
    {
        private readonly IPhraseService _phraseService;

        public PhrasesController(IPhraseService phraseService)
        {
            _phraseService = phraseService;
        }

        //TODO: What async Task<ActionResult>? Non Blocking Task I need an explanation on it and how its commonoly useed in the industry
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhraseDTO>>> GetAll()
        {
            var phrases = await _phraseService.GetAllPhrasesAsync();
            return Ok(phrases);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PhraseDTO>> GetById(int id)
        {
            try
            {
                var phrase = await _phraseService.GetPhraseByIdAsync(id);
                return Ok(phrase);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("theme/{themeId}")]
        public async Task<ActionResult<IEnumerable<PhraseDTO>>> GetByTheme(int themeId)
        {
            var phrases = await _phraseService.GetPhrasesByThemeAsync(themeId);
            return Ok(phrases);
        }
    }
}
