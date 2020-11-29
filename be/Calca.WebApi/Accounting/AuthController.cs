using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calca.WebApi.Accounting
{
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        [HttpGet("external-login")]
        public async Task<IActionResult> ExternalLogin(string provider)
        {
            if (!string.Equals(provider, GoogleDefaults.AuthenticationScheme, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest($"Only {GoogleDefaults.AuthenticationScheme} provider is supported");
            }

            if (User.Identity.IsAuthenticated)
                return Redirect("/");

            return Challenge(GoogleDefaults.AuthenticationScheme);
        }
    }
}
