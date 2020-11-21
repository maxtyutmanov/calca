using Calca.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calca.IntegrationTests.Fixture
{
    public class ControlledSystemClock : ISystemClock
    {
        private DateTime? _utcNowOverride;

        public DateTime UtcNow => _utcNowOverride ?? DateTime.UtcNow;

        public void Override(DateTime utcNow) => _utcNowOverride = utcNow;
    }
}
