using System;
using System.Collections.Generic;
using System.Text;

namespace Calca.Domain.Accounting
{
    /// <summary>
    /// Party of the ledger
    /// </summary>
    public class LedgerMember
    {
        public long LedgerId { get; private set; }

        public long UserId { get; private set; }

        public LedgerMember(long ledgerId, long userId)
        {
            LedgerId = ledgerId;
            UserId = userId;
        }

        private LedgerMember() { }
    }
}
