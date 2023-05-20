using AccuFin.Api.Models;
using AccuFin.Data;
using AccuFin.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace AccuFin.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("administration")]
    public class AdministrationController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly AdministrationRepository _administrationRepository;

        public AdministrationController(AccuFinDatabaseContext accuFinDatabaseContext
            , UserRepository userRepository
            , AdministrationRepository administrationRepository)
            : base()
        {
            _userRepository = userRepository;
            _administrationRepository = administrationRepository;
        }
        [HttpGet("{id}")]
        [Authorize(Policy = Policy.Administrator)]
        public async Task<ActionResult<AdministrationModel>> GetItemByIdAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var item = await _administrationRepository.GetItemByIdAsync(id);
            if (item == null)
            {
                return BadRequest("Niet gevonden");
            }
            //parse orderby;
            return Ok(item);
        }

        [HttpPost("{id}")]
        [Authorize(Policy = Policy.Administrator)]
        public async Task<ActionResult<AdministrationModel>> EditItemById(Guid id, [FromBody] AdministrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var item = await _administrationRepository.EditItemAsync(id, model);
            //parse orderby;
            if (item == null)
            {
                return BadRequest("Niet gevonden");
            }
            //parse orderby;
            return Ok(item);
        }
        [HttpPost("delete/{id}")]
        [Authorize(Policy = Policy.Administrator)]
        public async Task<ActionResult<AdministrationModel>> EditItemById(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var isDeleted = await _administrationRepository.DeleteItemAsync(id);
            //parse orderby;
            if (isDeleted == false)
            {
                return BadRequest("Niet gevonden");
            }
            //parse orderby;
            return Ok(isDeleted);
        }
        

        [HttpGet]
        [Authorize(Policy = Policy.Administrator)]
        public async Task<ActionResult<FinCollection<AdministrationCollectionItem>>> GetCollectionAsync([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string orderBy = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //parse orderby;
            return await _administrationRepository.GetCollectionAsync(page, pageSize, orderBy?.Split(','));
        }

        [HttpPost]
        [Authorize(Policy = Policy.Administrator)]
        public async Task<ActionResult<AdministrationModel>> AddAdministrationAsync([FromBody] AdministrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _administrationRepository.AddItemAsync(model));
        }
    }
}
