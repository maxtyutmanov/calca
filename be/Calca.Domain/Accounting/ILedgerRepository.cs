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

        Task<IReadOnlyList<LedgerListItem>> GetByCreatorId(long userId, CancellationToken ct);

        Task<IReadOnlyList<LedgerListItem>> GetByMemberId(long userId, CancellationToken ct);

        Task Add(Ledger ledger, CancellationToken ct);

        Task Update(Ledger ledger, long version, CancellationToken ct);
    }
}
