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
                o.AddPolicy(
                    KnownAuthzPolicies.AllowLedgerMetaEdit, 
                    policy => policy.Combine(authenticatedOnlyPolicy).AddRequirements(new LedgerSameOwnerRequirement()));
                o.AddPolicy(
                    KnownAuthzPolicies.AllowLedgerView,
                    policy => policy.Combine(authenticatedOnlyPolicy).AddRequirements(new LedgerParticipantRequirement()));
                o.AddPolicy(
                    KnownAuthzPolicies.AllowLedgerOperationsEdit, 
                    policy => policy.Combine(authenticatedOnlyPolicy).AddRequirements(new LedgerParticipantRequirement()));
            });

            services.AddSingleton<IAuthorizationHandler, LedgerSameOwnerHandler>();
            services.AddSingleton<IAuthorizationHandler, LedgerParticipantHandler>();

            return services;
        }
    }
}
