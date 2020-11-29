using Calca.Domain;
using Calca.Domain.Users;
using Calca.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Calca.WebApi.Auth
{
    public static class AuthenticationServiceCollectionExt
    {
        public static IServiceCollection AddCustomizedAuthentication(this IServiceCollection services, IConfiguration config)
        {
            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie()
                .AddGoogle(o =>
                {
                    config.GetSection("Auth:Google").Bind(o);
                    o.Events.OnCreatingTicket = async ctx =>
                    {
                        var services = ctx.HttpContext.RequestServices;
                        var principal = ctx.Principal;
                        var ct = ctx.HttpContext.RequestAborted;
                        await AddUserIdClaimToPrincipal(services, principal, ct);
                    };
                    //o.Events.OnTicketReceived = async ctx =>
                    //{
                    //    ctx.HandleResponse();
                    //    ctx.HttpContext.Response.Redirect("/");
                    //    await ctx.HttpContext.Response.CompleteAsync();
                    //};
                });

            return services;
        }

        private static async Task<ClaimsPrincipal> AddUserIdClaimToPrincipal(IServiceProvider services, ClaimsPrincipal principal, CancellationToken ct)
        {
            var uow = services.GetRequiredService<IUnitOfWork>();
            var userSvc = services.GetRequiredService<IUserService>();
            var user = await userSvc.GetOrAddUser(principal, ct);
            (principal.Identity as ClaimsIdentity).AddClaim(new Claim("CalcaUserId", user.Id.ToString()));
            await uow.Commit(ct);
            return principal;
        }
    }
}
