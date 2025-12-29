using E_Commerce.Service.Abstraction.Interfaces;
using E_Commerce.Shared.Common;
using E_Commerce.Shared.DTOs.AuthDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ApiBaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var result = await _authService.LoginAsync(loginDTO);
            // service should return ErrorType.InvalidCrendentials on bad login
            return FromResult(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            var result = await _authService.RegisterAsync(registerDTO);
            // on failure, service returns Validation/Failure errors
            return FromResult(result);
        }
        [HttpGet("currentUser")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {

            var email = User.FindFirstValue(ClaimTypes.Email);
            if (email == null)
            {
                return Unauthorized();
            }
            var result = await _authService.GetUserByEmail(email);
            return FromResult(result);
        }
        [HttpGet("emailExists")]
        public async Task<IActionResult> CheckEmailExists([FromQuery] string email)
        {
            var result = await _authService.CheckEmailExist(email);
            return FromResult(result);
        }

    }
}
