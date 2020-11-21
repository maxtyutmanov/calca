using Calca.Domain;
using Calca.Domain.Accounting;
using Calca.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
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
            var ledger = await _ctx.Ledgers.Include(x => x.Members).FirstOrDefaultAsync(x => x.Id == id, ct);
            return ledger;
        }

        public Task Add(Ledger ledger, CancellationToken ct)
        {
            ledger.Version = 1;
            _ctx.Ledgers.Add(ledger);
            return _ctx.SaveChangesAsync(ct);
        }

        public Task Update(Ledger ledger, long version, CancellationToken ct)
        {
            _ctx.Entry(ledger).OriginalValues[nameof(Ledger.Version)] = version;
            // TODO: wrap concurrency exception
            ledger.IncrementVersion();
            _ctx.Ledgers.Update(ledger);
            return _ctx.SaveChangesAsync(ct);
        }
    }
}
