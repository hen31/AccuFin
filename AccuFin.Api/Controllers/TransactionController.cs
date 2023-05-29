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
    [Route("transaction")]
    public class TransactionController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly TransactionRepository _transactionRepository;
        private readonly NordigenClient _nordigenClient;
        private readonly IWebHostEnvironment _enviroment;

        public TransactionController(AccuFinDatabaseContext accuFinDatabaseContext
            , UserRepository userRepository
            , TransactionRepository transactionRepository)
            : base()
        {
            _userRepository = userRepository;
            _transactionRepository = transactionRepository;
        }

        [HttpGet("{AdministrationId}")]
        [Authorize]
        public async Task<ActionResult<FinCollection<TransactionCollectionItem>>> GetCollectionAsync(Guid administrationId, [FromQuery] int page, [FromQuery] int pageSize, [FromQuery] string orderBy = null, [FromQuery] string singleSearch = null)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //parse orderby;
            return await _transactionRepository.GetCollectionAsync(administrationId, page, pageSize, orderBy?.Split(','), singleSearch);
        }
    }
}
