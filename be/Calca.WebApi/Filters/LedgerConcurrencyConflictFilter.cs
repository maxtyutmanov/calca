using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calca.WebApi.Filters
{
    public class LedgerConcurrencyConflictFilter : ConcurrencyConflictFilter
    {
        public LedgerConcurrencyConflictFilter() : base("ledger")
        {
        }

        protected override string Message => 
            "An attempt was made to modify a ledger based on outdated information. Please reload the ledger and try again.";
    }
}
