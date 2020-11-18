using Calca.Domain.Accounting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calca.Domain.Accounting
{
    public class BalanceItem
    {
        public Member Member { get; }

        public decimal Balance { get; }

        public BalanceItem(Member member, decimal balance)
        {
            Member = member;
            Balance = balance;
        }
    }
}
