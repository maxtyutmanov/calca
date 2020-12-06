using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Calca.WebApi.Accounting
{
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        [HttpGet("user")]
        public IActionResult GetUserInfo()
        {
            if (User.Identity.IsAuthenticated)
            {
                var id = User.FindFirstValue("CalcaUserId");
                var name = User.FindFirstValue(ClaimTypes.Name);
                var email = User.FindFirstValue(ClaimTypes.Email);

                return Ok(new { id, name, email });
            }

            return NoContent();
        }

        [HttpGet("external-login")]
        public async Task<IActionResult> ExternalLogin(string provider, string returnUrl)
        {
            if (!string.Equals(provider, GoogleDefaults.AuthenticationScheme, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest($"Only {GoogleDefaults.AuthenticationScheme} provider is supported");
            }

            if (!Url.IsLocalUrl(returnUrl))
            {
                returnUrl = "/";
            }

            return Challenge(
                new AuthenticationProperties()
                {
                    RedirectUri = returnUrl
                },
                GoogleDefaults.AuthenticationScheme);
        }
    }
}
