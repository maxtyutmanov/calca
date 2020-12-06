using Calca.Domain.Accounting;
using Calca.WebApi.Auth;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Calca.WebApi.Authorization
{
    public class LedgerSameOwnerRequirement : IAuthorizationRequirement
    {
    }

    public class LedgerSameOwnerHandler : AuthorizationHandler<LedgerSameOwnerRequirement, Ledger>

    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
                                                       LedgerSameOwnerRequirement requirement,
                                                       Ledger resource)
        {
            var currentUserIdStr = context.User.FindFirstValue(KnownClaimTypes.UserId);
            if (long.TryParse(currentUserIdStr, out var currentUserId) &&
                currentUserId == resource.CreatorId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
