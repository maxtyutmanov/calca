using Calca.Domain.Accounting;
using Calca.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Calca.Infrastructure.Repo
{
    public class LedgerRepository : ILedgerRepository
    {
        private readonly CalcaDbContext _ctx;

        public LedgerRepository(CalcaDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<Ledger> GetById(long id, CancellationToken ct)
        {
            return await _ctx.FindAsync<Ledger>(new object[] { id }, ct);
        }

        public Task Add(Ledger ledger, CancellationToken ct)
        {
            _ctx.Ledgers.Add(ledger);
            return _ctx.SaveChangesAsync(ct);
        }

        public Task Update(Ledger ledger, CancellationToken ct)
        {
            _ctx.Ledgers.Update(ledger);
            return _ctx.SaveChangesAsync(ct);
        }
    }
}
