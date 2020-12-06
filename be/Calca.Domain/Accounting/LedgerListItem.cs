using System;
using System.Collections.Generic;
using System.Text;

namespace Calca.Domain.Accounting
{
    public class LedgerListItem
    {
        public long Id { get; }

        public string Name { get; }

        public long CreatorId { get; }

        public DateTime CreatedAt { get; }

        public LedgerListItem(long id, string name, long creatorId, DateTime createdAt)
        {
            Id = id;
            Name = name;
            CreatorId = creatorId;
            CreatedAt = createdAt;
        }
    }
}
