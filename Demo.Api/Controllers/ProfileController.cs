using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Demo.Core.Interfaces;
using Demo.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Demo.Api.Models.Profile;
using Demo.Core.Models.Profile;

namespace Demo.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ProfileController: BaseController
    {
        private readonly IUserService _userService;

        public ProfileController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _userService.GetUserAsync<ProfileResult>(GetUserId());

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] ProfileRequest request)
        {
            var action = request.ToAction(GetUserId());
            var result = await _userService.UpdateUserAsync(action);

            return Ok(result);
        }

        [HttpPost]
        [Route("Refresh/Confirmcode/Email")]
        public async Task<IActionResult> RefreshCode()
        {
            await _userService.RefreshUserConfirmCodeAsync(GetUserId());

            return Ok(new SuccessResponse { Success = true });
        }

        [HttpPost]
        [Route("Confirm/Email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
        {
            var action = request.ToAction(GetUserId());
            var result = await _userService.ConfirmEmailAsync(action);

            return Ok(result);
        }

        [HttpPost]
        [Route("Device")]
        public async Task<IActionResult> SaveDevice([FromBody] DeviceRequest request)
        {
            var action = request.ToAction(GetUserId());
            await _userService.AddDeviceAsync(action);

            return Ok(new SuccessResponse { Success = true });
        }
    }
}
