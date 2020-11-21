using System;
using System.Collections.Generic;
using System.Text;

namespace Calca.Domain.Accounting
{
    public class LedgerOperation
    {
        public long Id { get; private set; }
        public long LedgerId { get; set; }
        public string Description { get; private set; }
        public List<OperationMember> Members { get; private set; }
        public decimal Amount { get; private set; }
        public long CreatorId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        private LedgerOperation() { }

        public LedgerOperation(long ledgerId, string description, List<OperationMember> members, decimal amount, long creatorId, DateTime createdAt)
        {
            LedgerId = ledgerId;
            Description = description;
            Members = members;
            Amount = amount;
            CreatorId = creatorId;
            CreatedAt = createdAt;
        }
    }
}
