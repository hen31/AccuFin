using AccuFin.Api.Models;
using AccuFin.Api.Services.BankIntegration;
using AccuFin.Data;
using AccuFin.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AccuFin.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("administration")]
    public class AdministrationController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly AdministrationRepository _administrationRepository;
        private readonly NordigenClient _nordigenClient;
        private readonly IWebHostEnvironment _enviroment;

        public AdministrationController(AccuFinDatabaseContext accuFinDatabaseContext
            , UserRepository userRepository
            , AdministrationRepository administrationRepository,
            NordigenClient nordigenClient,
            IWebHostEnvironment enviroment)
            : base()
        {
            _userRepository = userRepository;
            _administrationRepository = administrationRepository;
            _nordigenClient = nordigenClient;
            _enviroment = enviroment;
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
            foreach (var account in item.BankAccounts)
            {
                var transactions = await _nordigenClient.GetTransactionsAsync(account.AccountId);
            }
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
            var item = await _administrationRepository.GetItemByIdAsync(id);
            if (item == null) { return null; }

            await ProcessAdministrationImage(model);
            item = await _administrationRepository.EditItemAsync(id, model);
            RenameImageIfNeeded(model, item);

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
        public async Task<ActionResult<AdministrationModel>> DeleteItemById(Guid id)
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
        public async Task<ActionResult<FinCollection<AdministrationCollectionItem>>> GetCollectionAsync([FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string orderBy = null, [FromQuery] string singleSearch = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //parse orderby;
            return await _administrationRepository.GetCollectionAsync(page, pageSize, orderBy?.Split(','), singleSearch);
        }
        [HttpGet("myadministrations")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<AdministrationCollectionItem>>> GetMyAdministrationsAsync()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(await _administrationRepository.GetMyAdministrationsAsync(this.GetFinUserId()));
        }


        [HttpPost]
        [Authorize(Policy = Policy.Administrator)]
        public async Task<ActionResult<AdministrationModel>> AddAdministrationAsync([FromBody] AdministrationModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await ProcessAdministrationImage(model);
            var item = await _administrationRepository.AddItemAsync(model);
            RenameImageIfNeeded(model, item);
            return Ok(item);
        }

        private void RenameImageIfNeeded(AdministrationModel model, AdministrationModel item)
        {
            if (!string.IsNullOrWhiteSpace(model.ImageFileName) && item.Id != Guid.Empty)
            {
                try
                {
                    System.IO.File.Move(_enviroment.WebRootPath + "/filestore/images/" + model.ImageFileName, _enviroment.WebRootPath + "/filestore/images/" + item.Id.ToString() + ".png", true);
                }
                catch (Exception ex)
                {
                }
            }

        }

        private async Task ProcessAdministrationImage(AdministrationModel model)
        {
            try
            {
                if (model.ImageData != null)
                {
                    model.ImageFileName = Guid.NewGuid().ToString() + ".png";
                    Directory.CreateDirectory(_enviroment.WebRootPath + "/filestore/images/");
                    await System.IO.File.WriteAllBytesAsync(_enviroment.WebRootPath + "/filestore/images/" + model.ImageFileName, Convert.FromBase64String(model.ImageData));
                }
            }
            catch (Exception ex)
            {
                model.ImageFileName = null;
                model.ImageData = null;
            }
        }
    }
}
