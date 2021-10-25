using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Demo.Api.Models.Auth;
using Demo.Core.Interfaces;
using Demo.Api.Models;
using Microsoft.AspNetCore.Authorization;

namespace Demo.Api.Controllers
{
    [ApiController]
    public class AuthController: Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("Token")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var action = request.ToAction();
            var result = await _authService.LoginAsync(action);
            var response = new AuthResponse(result);

            return Ok(response);
        }

        [HttpPost]
        [Route("Registration")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var action = request.ToAction();
            var result = await _authService.RegisterAsync(action);
            var response = new AuthResponse(result);

            return Ok(response);
        }

        [HttpPost]
        [Route("Token/Refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {

            var action = request.ToAction();
            var result = await _authService.RefreshTokenAsync(action);
            var response = new AuthResponse(result);

            return Ok(response);
        }

        [HttpPost]
        [Route("Restore/Email")]
        public async Task<IActionResult> RestorePassword([FromBody] RestorePasswordRequest request)
        {
            var action = request.ToAction();
            await _authService.RestorePasswordAsync(action);

            return Ok(new SuccessResponse { Success = true });            
        }
    }
}
