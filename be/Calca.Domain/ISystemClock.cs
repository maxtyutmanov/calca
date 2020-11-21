using System;
using System.Collections.Generic;
using System.Text;

namespace Calca.Domain
{
    public interface ISystemClock
    {
        DateTime UtcNow { get; }
    }
}
