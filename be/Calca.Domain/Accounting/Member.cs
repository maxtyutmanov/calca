using System;
using System.Collections.Generic;
using System.Text;

namespace Calca.Domain.Accounting
{
    /// <summary>
    /// Party of the ledger
    /// </summary>
    public class Member
    {
        public long Id { get; }

        public long UserId { get; }

        public string Name { get; }

        public Member(long userId, string name, long id = 0)
        {
            Id = id;
            UserId = userId;
            Name = name;
        }
    }
}
