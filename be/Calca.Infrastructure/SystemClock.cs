using Calca.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calca.Infrastructure
{
    public class SystemClock : ISystemClock
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
