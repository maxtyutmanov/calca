using System;
using System.Collections.Generic;
using System.Security;
using System.Security.Claims;
using System.Text;

namespace Calca.Infrastructure.Utils
{
    public static class ClaimsPrincipalExt
    {
        public static long GetUserId(this ClaimsPrincipal user)
        {
            if (!user.Identity.IsAuthenticated)
                throw new SecurityException("User is not authenticated");

            var idStr = user.FindFirst(KnownClaimTypes.UserId)?.Value;
            if (string.IsNullOrEmpty(idStr))
                throw new SecurityException($"No {KnownClaimTypes.UserId} claim is found");

            if (!long.TryParse(idStr, out var id))
                throw new SecurityException($"{KnownClaimTypes.UserId} value has invalid format. It must be Int64");

            return id;
        }
    }
}
