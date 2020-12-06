using Calca.Domain.Accounting;
using Calca.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Calca.WebApi.Authorization
{
    public class LedgerParticipantRequirement : IAuthorizationRequirement
    {
    }

    public class LedgerParticipantHandler : AuthorizationHandler<LedgerParticipantRequirement, Ledger>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, LedgerParticipantRequirement requirement, Ledger resource)
        {
            var currentUserIdStr = context.User.FindFirstValue(KnownClaimTypes.UserId);
            if (long.TryParse(currentUserIdStr, out var currentUserId) &&
                resource.Members.Any(m => m.UserId == currentUserId))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
