using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Calca.WebApi.Authorization
{
    public static class AuthorizationServiceCollectionExt
    {
        public static IServiceCollection AddCustomizedAuthorization(this IServiceCollection services)
        {
            var authenticatedOnlyPolicy = new AuthorizationPolicyBuilder(CookieAuthenticationDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build();

            services.AddAuthorization(o =>
            {
                o.DefaultPolicy = authenticatedOnlyPolicy;

                var ledgerAccessPolicy = new AuthorizationPolicyBuilder(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddRequirements(new LedgerParticipantRequirement())
                    .Build();

                o.AddPolicy(
                    KnownAuthzPolicies.AllowLedgerMetaEdit, 
                    ledgerAccessPolicy);
                o.AddPolicy(
                    KnownAuthzPolicies.AllowLedgerView,
                    ledgerAccessPolicy);
                o.AddPolicy(
                    KnownAuthzPolicies.AllowLedgerOperationsEdit,
                    ledgerAccessPolicy);
            });

            services.AddSingleton<IAuthorizationHandler, LedgerParticipantHandler>();

            return services;
        }
    }
}
