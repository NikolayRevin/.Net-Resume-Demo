using System;
using System.Threading.Tasks;
using Demo.Api.Models.User;
using Demo.Core.Interfaces;
using Demo.Core.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController: BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _userService.GetUserListAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var user = await _userService.GetUserAsync<UserResult>(id);

            return Ok(user);
        }

        [HttpPost("{id}/vote")]
        public async Task<IActionResult> Vote(string id, [FromBody] UserVoteRequest request)
        {
            var action = request.ToAction(id, GetUserId());
            var result = await _userService.VoteForUserAsync(action);

            return Ok(result);
        }
    }
}
