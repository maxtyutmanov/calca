using System;
using System.Collections.Generic;
using System.Text;

namespace Calca.Domain
{
    public interface ISecurityContext
    {
        long CurrentUserId { get; }
    }
}
