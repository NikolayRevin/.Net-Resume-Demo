using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace Demo.Api.Controllers
{
    public abstract class BaseController: Controller
    {
        [ApiExplorerSettings(IgnoreApi = true)]
        public string GetUserId()
        {
            if (User is null)
            {
                throw new ArgumentNullException(nameof(User));
            }

            return User.FindFirst(ClaimTypes.Sid).Value;
        }
    }
}
