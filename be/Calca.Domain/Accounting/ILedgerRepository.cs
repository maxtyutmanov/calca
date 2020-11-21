using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Calca.Domain.Accounting
{
    public interface ILedgerRepository
    {
        Task<Ledger> GetById(long id, CancellationToken ct);

        Task Add(Ledger ledger, CancellationToken ct);

        Task Update(Ledger ledger, CancellationToken ct);
    }
}
