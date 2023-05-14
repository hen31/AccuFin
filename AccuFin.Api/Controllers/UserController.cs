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
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public UserController(AccuFinDatabaseContext accuFinDatabaseContext, UserRepository userRepository) : base()
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<ActionResult<CurrentUserModel>> GetCurrentUserAsync()
        {
            string emailAdress = HttpContext.GetCurrentUserEmail();
            if(string.IsNullOrWhiteSpace(emailAdress))
            {
                return BadRequest();
            }
            return Ok(await _userRepository.GetOrCreateCurrentUser(emailAdress));
        }


        [HttpPost]
        public async Task<ActionResult<CurrentUserModel>> UpdateCurrentUserAsync([FromBody]CurrentUserModel userModel)
        {
            string emailAdress = HttpContext.GetCurrentUserEmail();
            if (string.IsNullOrWhiteSpace(emailAdress))
            {
                return BadRequest();
            }
            var updateResult = await _userRepository.UpdateUserAsync(emailAdress, userModel);
            if(updateResult==null)
            {
                return BadRequest();
            }
            return Ok(updateResult);
        }
    }
}
