using System;
using System.Collections.Generic;
using System.Text;

namespace Calca.Domain.Accounting
{
    public class BalanceReport
    {
        public ICollection<BalanceItem> Items { get; }

        public BalanceReport(ICollection<BalanceItem> items)
        {
            Items = items;
        }
    }
}
