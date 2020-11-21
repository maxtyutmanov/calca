using System;
using System.Collections.Generic;
using System.Text;

namespace Calca.Domain.Accounting
{
    public class Ledger
    {
        public long Id { get; private set; }
        public string Name { get; private set; }
        public List<LedgerMember> Members { get; private set; }
        public long Version { get; set; }
        public long CreatorId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private Ledger() { }

        public Ledger(string name, List<LedgerMember> members, long creatorId, DateTime createdAt)
        {
            Name = name;
            Members = members;
            CreatorId = creatorId;
            CreatedAt = createdAt;
        }

        public void IncrementVersion() => Version++;
    }
}
