using Calca.Domain;
using Calca.Infrastructure.Utils;
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

        public long CurrentUserId => _httpCtxAccessor.HttpContext.User.GetUserId();
    }
}
