using Authentication.api.DTO;
using Authentication.api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authentication.api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<DWUser> userManager;
        private readonly SignInManager<DWUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(IConfiguration configuration,
            UserManager<DWUser> userManager,
            SignInManager<DWUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.configuration = configuration;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        [HttpPost("Roles")]
        public async Task<ActionResult> CreateRole(string role)
        {
            await roleManager.CreateAsync(new IdentityRole(role));
            return NoContent();
        }

        [HttpPost("AddUserToRol")]
        public async Task<ActionResult> AddUserToRol(string role, string username)
        {
            var user = await userManager.FindByNameAsync(username);

            await userManager.AddToRoleAsync(user, role);

            return NoContent();
        }

        private async Task<bool> UserExists(string username)
            => await userManager.Users.AnyAsync(x => x.UserName == username);

        private async Task<string> GetToken(DWUser user)
        {
            var now = DateTime.UtcNow;
            var key = configuration.GetValue<string>("Identity:Key");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti,user.Id),
                new Claim(JwtRegisteredClaimNames.Iat,now.ToUniversalTime().ToString(), ClaimValueTypes.Integer64),
                new Claim(JwtRegisteredClaimNames.Email,user.Email!)
            };

            var roles = await userManager.GetRolesAsync(user);

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var signinKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key!));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = now.AddMinutes(15),
                SigningCredentials = new SigningCredentials(signinKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var encodedJWT = new JwtSecurityTokenHandler();

            var token = encodedJWT.CreateToken(tokenDescriptor);

            return encodedJWT.WriteToken(token);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO registerDTO)
        {
            if (await UserExists(registerDTO.UserName.ToUpper()))
                return BadRequest("User already exists");

            var user = new DWUser
            {
                UserName = registerDTO.UserName.ToUpper(),
                Email = registerDTO.Email,
                Badge = registerDTO.Badge,
                Position = registerDTO.Position,
            };

            var result = await userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);


            return Ok(new UserDTO { UserName = registerDTO.UserName, Token = await GetToken(user) });
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            if (!await UserExists(loginDTO.UserName.ToUpper()))
                return Unauthorized();

            var user = await userManager.Users.SingleAsync(x => x.UserName ==
            loginDTO.UserName.ToUpper());

            var result = await signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, true);

            if (!result.Succeeded)
                return Unauthorized();

            return Ok(new UserDTO {Token = await GetToken(user), UserName = user.UserName! });
        }
    }
}
