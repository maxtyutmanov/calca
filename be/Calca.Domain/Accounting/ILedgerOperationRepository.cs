using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Calca.Domain.Accounting
{
    public interface ILedgerOperationRepository
    {
        Task<IReadOnlyList<LedgerOperation>> GetByLedger(long ledgerId, CancellationToken ct);

        Task<long> AddOperation(LedgerOperation operation, CancellationToken ct);
    }
}
