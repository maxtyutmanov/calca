using Calca.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace Calca.Infrastructure
{
    public class SecurityContext : ISecurityContext
    {
        private readonly IHttpContextAccessor _httpCtxAccessor;

        public SecurityContext(IHttpContextAccessor httpCtxAccessor)
        {
            _httpCtxAccessor = httpCtxAccessor;
        }

        public long CurrentUserId
        {
            get
            {
                var user = _httpCtxAccessor.HttpContext.User;
                if (!user.Identity.IsAuthenticated)
                    throw new SecurityException("User is not authenticated");

                var idStr = user.FindFirst("CalcaUserId")?.Value;
                if (string.IsNullOrEmpty(idStr))
                    throw new SecurityException("No CalcaUserId claim is found");

                if (!long.TryParse(idStr, out var id))
                    throw new SecurityException("CalcaUserId value has invalid format. It must be integer");

                return id;
            }
        }
    }
}
