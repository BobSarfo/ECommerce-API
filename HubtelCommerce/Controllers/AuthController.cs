using HubtelCommerce.DAL;
using HubtelCommerce.Dtos;
using HubtelCommerce.Services;
using HubtelCommerce.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace HubtelCommerce.Controllers
{
    [Route("api/auth")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly JWT _jwtConfig;
        private readonly UserManager<AppUser> _userManager;

        public AuthController(UserManager<AppUser> userManager,IOptions<JWT> jwtConfig)
        {
            _jwtConfig = jwtConfig.Value;
            _userManager = userManager;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response))]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {

            if (loginDto == null)
            {
                return BadRequest(new Response { Succeeded = false, Message = "Invalid data request." });
            }            

            AppUser user = await _userManager.FindByNameAsync(loginDto.Username.ConvertToLocalFormat());

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                return BadRequest(new Response { Succeeded = false, Message = "Invalid Username or Password" });
            }

            var token = await TokenService.GenerateTokenAsync(_userManager, _jwtConfig, user);

            return Ok(new LoginResponse { Username = user.UserName, Token = token });
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Response))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response))]
        public async Task<IActionResult> RegisterNewUser(RegisterDto registerDto)
        {

            if (registerDto.ConfirmPassword != registerDto.Password)
            {
                return BadRequest(new Response { Status = "Error", Message = "Passwords do not match" });
            }
            if (registerDto == null)
            {
                return BadRequest(new Response { Status = "Error", Message = "Invalid data request." });
            }
            var existingUser = await _userManager.FindByNameAsync(registerDto.PhoneNumber.ConvertToLocalFormat());
            
            if (existingUser != null)
                return BadRequest(new Response { Status = "Error", Message = "PhoneNumber exists, please try another name" });

            var user = new AppUser
            {
                UserName = registerDto.PhoneNumber.ConvertToLocalFormat(),              
                PhoneNumber = registerDto.PhoneNumber.Trim(),                
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);


            if (!result.Succeeded)
            {
                var errorResponse = new Response { Status = "Validation Errors", Message = "" };
                foreach (var error in result.Errors)
                {
                    errorResponse.Message = string.Join(",", errorResponse.Message, error.Description);
                }
                return BadRequest(errorResponse);
            }

            return Created("/", new Response { Succeeded = true, Message = "User created successfully", Status = "Success" });
        }
    }
}
