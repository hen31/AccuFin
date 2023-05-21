using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AccuFin.Api.Data;
using AccuFin.Api.Areas.Identity.Data;
using AccuFin.Api.Models.Authentication;
using AccuFin.Api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;

namespace AccuFin.Api.Controllers.Authentication
{
    [ApiController]
    [Route("Authentication")]
    public class AuthenticationController : Controller
    {
        private readonly IdentityContext _appDbContext;
        private readonly UserManager<AccuFinUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthenticationController(UserManager<AccuFinUser> userManager,
            IdentityContext appDbContext, IEmailSender emailSender,
            IConfiguration configuration, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
            _emailSender = emailSender;
            _configuration = configuration;
            _roleManager = roleManager;
        }

        [HttpGet("makeadmin")]
        public async Task<ActionResult<bool>> GetCurrentUserAsync()
        {
            if (await _roleManager.RoleExistsAsync(Roles.Administrator) == false)
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.Administrator));
            }
            var user = await _userManager.FindByEmailAsync("hendrikdejonge@hotmail.com");
            await AddToRole(user, Roles.Administrator);
            return Ok(true);
        }

        private async Task AddToRole(AccuFinUser user, string role)
        {
            if (await _userManager.IsInRoleAsync(user, role) == false)
            {
                await _userManager.AddToRoleAsync(user, role);
            }
        }

        [HttpGet]
        [Authorize]
        [Route("me")]
        public async Task<IActionResult> GetMe()
        {
            return Ok("Testing");
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModel model)
        {
            string refreshToken = model.Token;
            var authorization = Request.Headers[HeaderNames.Authorization];

            if (!AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
            {
                return BadRequest(new List<ValidationError>() { new ValidationError() { Description = "Unknown" } });
            }
            var scheme = headerValue.Scheme;
            if (scheme != "Bearer")
            {
                return BadRequest(new List<ValidationError>() { new ValidationError() { Description = "Unknown" } });
            }

            var token = headerValue.Parameter;

            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            if (jwtSecurityToken == null)
            {
                return BadRequest(new List<ValidationError>() { new ValidationError() { Description = "Unknown" } });
            }

            var user = await _userManager.FindByEmailAsync(jwtSecurityToken.Claims.FirstOrDefault(b => b.Type == ClaimTypes.Email).Value);
            if (user == null)
            {
                return Unauthorized(new List<ValidationError>() { new ValidationError() { Description = "Unknown" } });
            }
            var clientId = jwtSecurityToken.Claims.FirstOrDefault(b => b.Type == "client_id")?.Value;
            if (string.IsNullOrWhiteSpace(clientId))
            {
                return Unauthorized(new List<ValidationError>() { new ValidationError() { Description = "Unknown" } });
            }
            var storedToken = await _appDbContext.RefreshTokens.FirstOrDefaultAsync(b => b.RefreshToken == refreshToken && b.UserId == user.Id && b.Revoked == false && b.ClientId == clientId);
            if (storedToken == null)
            {
                return Unauthorized(new List<ValidationError>() { new ValidationError() { Description = "Unknown" } });
            }
            storedToken.Revoked = true;
            await _appDbContext.SaveChangesAsync();
            TokenResult tokenModel = await GenerateTokenModel(user, clientId);
            return Ok(tokenModel);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                if (!user.EmailConfirmed)
                {
                    return BadRequest(new List<ValidationError>() { new ValidationError() { Code = "NOTVER", Description = "Email nog niet bevestigd" } });
                }

                TokenResult tokenModel = await GenerateTokenModel(user, model.ClientId);

                return Ok(tokenModel);
            }
            return Unauthorized(new List<ValidationError>() { new ValidationError() { Code = "NOTVER", Description = "Onbekende combinatie van email en wachtwoord" } });
        }

        private async Task<TokenResult> GenerateTokenModel(AccuFinUser user, string clientId)
        {
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("client_id", clientId)
                };
            
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = GetToken(authClaims);
            var refreshToken = GenerateRefreshToken(user, clientId);
            _appDbContext.RefreshTokens.Add(refreshToken);
            await _appDbContext.SaveChangesAsync();
            var tokenModel = new TokenResult
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                RefreshToken = refreshToken.RefreshToken
            };
            return tokenModel;
        }

        private StoredRefreshToken GenerateRefreshToken(AccuFinUser user, string clientId)
        {
            return new StoredRefreshToken()
            {
                UserId = user.Id,
                RefreshToken = GenerateRandomString(),
                ExpirationDate = DateTime.UtcNow.AddDays(7),
                ClientId = clientId
            };
        }

        private static string GenerateRandomString()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.UtcNow.AddMinutes(5),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        // POST api/accounts
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdentity = new AccuFinUser() { UserName = model.Email, Email = model.Email };

            var result = await _userManager.CreateAsync(userIdentity, model.Password);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(userIdentity);

            if (!result.Succeeded) return new BadRequestObjectResult(result.Errors);

            await _appDbContext.SaveChangesAsync();
            await _emailSender.SendEmailAsync(model.Email, "Emailadres bevestigen",
        $"Geachte meneer/mevrouw," +
        Environment.NewLine +
         Environment.NewLine +
        $"U kunt uw emailadres bevestigen met de volgende code: {code}" +
        Environment.NewLine +
            Environment.NewLine +
         $"Met vriendelijke groet," +
         Environment.NewLine +
         "AccuFin team");
            return Ok("Account created");
        }

        [HttpPost]
        [Route("ConfirmEmail")]
        public async Task<IActionResult> Post([FromBody] ConfirmEmailModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                return BadRequest(new List<ValidationError>() { new ValidationError() { Code = "INVCODE", Description = "Ongeldige code." } }); ;
            }
            var result = await _userManager.ConfirmEmailAsync(user, model.Code);

            if (!result.Succeeded) return new BadRequestObjectResult(result.Errors);

            await _appDbContext.SaveChangesAsync();

            return new OkObjectResult("Email confirmed");
        }


        [HttpPost]
        [Route("SendResetCode")]
        public async Task<IActionResult> Post([FromBody] ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                return new OkObjectResult("Passwordreset code send");
            }
            var result = await _userManager.GeneratePasswordResetTokenAsync(user);

            await _appDbContext.SaveChangesAsync();
            if (!model.ChangePassword)
            {
                await _emailSender.SendEmailAsync(model.Email, "Wachtwoord resetten",
                                                             $"Geachte meneer/mevrouw," +
                                                             Environment.NewLine +
                                                             Environment.NewLine +
                                                             $"U kunt uw wachtwoord resetten met de volgende code: {result}" +
                                                             Environment.NewLine +
                                                             Environment.NewLine +
                                                             $"Met vriendelijke groet," +
                                                             Environment.NewLine +
                                                             "BalanceKeeper team");
            }
            else
            {
                await _emailSender.SendEmailAsync(model.Email, "Wachtwoord wijzigen",
                                                                $"Geachte meneer/mevrouw," +
                                                                Environment.NewLine +
                                                                Environment.NewLine +
                                                                $"U kunt uw wachtwoord wijzigen met de volgende code: {result}" +
                                                                Environment.NewLine +
                                                                Environment.NewLine +
                                                                $"Met vriendelijke groet," +
                                                                Environment.NewLine +
                                                                "BalanceKeeper team");

            }
            return new OkObjectResult("Passwordreset code send");
        }


        [HttpPost]
        [Route("ResetPasswordWithCode")]
        public async Task<IActionResult> Post([FromBody] ResetPasswordWithCodeModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                return BadRequest();
            }
            var result = await _userManager.ResetPasswordAsync(user, model.ResetCode, model.Password);

            if (!result.Succeeded) return new BadRequestObjectResult(result.Errors);

            await _appDbContext.SaveChangesAsync();

            return new OkObjectResult("Password reset");
        }

    }
}