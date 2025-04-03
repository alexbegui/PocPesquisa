using AutoMapper;
using CensusFieldSurvey.DataBase;
using CensusFieldSurvey.Model.Common.Request;
using CensusFieldSurvey.Model.Common.Response;
using CensusFieldSurvey.Model.EntitesBD;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CensusFieldSurvey.API.Controllers
{
    [Route("api/[controller]")]
   // [Authorize]
    [ApiController]
    public class ResearchController(
        IRepository<Research> repositoryBD, IMapper mapper) : ControllerBase

    {
        private readonly IRepository<Research> _repositoryBD = repositoryBD;
        private readonly IMapper _mapper = mapper;
        private async Task<Research?> GetOrCreateInventoryEntry(ResearchRequest researchRequest)
        {
            if (researchRequest.Id <= 0)
            {
                return new Research
                {
                    Form = researchRequest.Form
                };
            }
            else
            {
                var itemSearch = await _repositoryBD.GetById(researchRequest.Id);
                return itemSearch;
            }
        }

        private async Task SaveInventoryEntry(Research newEntry, int id)
        {

            if (id <= 0)
            {
                await _repositoryBD.Add(newEntry);
            }
            else
            {
                await _repositoryBD.Update(newEntry);
            }
        }


        [HttpGet("GetResearchAll", Name = "GetResearchAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetResearchAll()
        {

            var listResearch = await _repositoryBD.GetAll();
            return Ok(listResearch);
        }


        [HttpPost("AddNewResearch", Name = "AddNewResearch")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddNewResearch([FromBody] ResearchRequest researchRequest)
        {

            // Verifica se o modelo é válido
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var newEntry = await GetOrCreateInventoryEntry(researchRequest);

                if (newEntry == null)
                {
                    return BadRequest("Não foi possível criar ou editar o item lançado.");
                }

                newEntry.Form = researchRequest.Form;

                await SaveInventoryEntry(newEntry, researchRequest.Id);

                var response = _mapper.Map<ResearchResponse>(newEntry);
                return CreatedAtRoute("AddNewResearch", new { id = response.IdResearch }, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}