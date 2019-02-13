using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DapperDino.Api.Models;
using DapperDino.DAL;
using DapperDino.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DapperDino.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountController : BaseController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _dbContext;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            ApplicationDbContext dbContext
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<object> Login([FromBody] LoginDto model)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                if (result.Succeeded)
                {
                    var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
                    return await GenerateJwtToken(model.Email, appUser);
                }

                throw new ApplicationException("INVALID_LOGIN_ATTEMPT");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }

        }

        [HttpPost]
        public async Task<object> Register([FromBody] RegisterDto model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email
            };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return await GenerateJwtToken(model.Email, user);
            }

            ModelState.AddModelError("EFErrors", string.Join(" ", result.Errors.Select(x => x.Description)));

            return BadRequest(ModelState);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RegisterDiscord([FromBody] RegistrationModel model)
        {
            var user =  _dbContext.ApplicationUsers.FirstOrDefault(x => x.DiscordRegistrationCode == Guid.Parse(model.RegistrationCode));

            if (user == null)
                return BadRequest("Registration code not found");

            if (user.RegisteredDiscordAccount)
            {
                if (model.IsHappyToHelp)
                {
                    await _userManager.AddToRoleAsync(user, RoleNames.HappyToHelp);
                }
                return BadRequest("This account has already registered a discord account");
            }

            DiscordUser discordUser = null;

            discordUser = _dbContext.DiscordUsers.FirstOrDefault(x => x.DiscordId == model.DiscordId);

            if (discordUser == null)
            {
                discordUser = new DiscordUser() { DiscordId = model.DiscordId, Username = model.Username, Level = 0, Xp = 0 };
                _dbContext.DiscordUsers.Add(discordUser);

                await _dbContext.SaveChangesAsync();
            }

            user.DiscordUserId = discordUser.Id;
            user.RegisteredDiscordAccount = true;

            await _dbContext.SaveChangesAsync();

            if (model.IsHappyToHelp)
            {
                await _userManager.AddToRoleAsync(user, RoleNames.HappyToHelp);
            }

            return Ok(discordUser);
        }

        private async Task<object> GenerateJwtToken(string email, IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public class LoginDto
        {
            [Required]
            public string Email { get; set; }

            [Required]
            public string Password { get; set; }

        }

        public class RegisterDto
        {
            [Required]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "PASSWORD_MIN_LENGTH", MinimumLength = 6)]
            public string Password { get; set; }
        }
    }
}
