using System;
using System.Collections.Generic;
using System.Text;

namespace Calca.Domain.Accounting
{
    public class LedgerOperation
    {
        public long Id { get; private set; }
        public string Description { get; private set; }
        public List<OperationMember> Members { get; private set; }
        public long CreatorId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private LedgerOperation() { }

        public LedgerOperation(string description, List<OperationMember> members, long creatorId, DateTime createdAt)
        {
            Description = description;
            Members = members;
            CreatorId = creatorId;
            CreatedAt = createdAt;
        }
    }
}
