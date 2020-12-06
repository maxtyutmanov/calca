using Calca.Domain;
using Calca.Domain.Accounting;
using Calca.Infrastructure.Context;
using Calca.Infrastructure.Errors;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IReadOnlyList<LedgerListItem>> GetByCreatorId(long creatorId, CancellationToken ct)
        {
            var query = 
                from ledger in _ctx.Ledgers
                where ledger.CreatorId == creatorId
                select new LedgerListItem(ledger.Id, ledger.Name, ledger.CreatorId, ledger.CreatedAt);

            return await query.ToListAsync(ct);
        }

        public async Task<IReadOnlyList<LedgerListItem>> GetByMemberId(long userId, CancellationToken ct)
        {
            var query =
                from ledger in _ctx.Ledgers.Include(l => l.Members)
                where ledger.Members.Any(m => m.UserId == userId)
                select new LedgerListItem(ledger.Id, ledger.Name, ledger.CreatorId, ledger.CreatedAt);

            return await query.ToListAsync(ct);
        }

        public Task Add(Ledger ledger, CancellationToken ct)
        {
            ledger.Version = 1;
            _ctx.Ledgers.Add(ledger);
            return _ctx.SaveChangesAsync(ct);
        }

        public async Task Update(Ledger ledger, long version, CancellationToken ct)
        {
            _ctx.Entry(ledger).OriginalValues[nameof(Ledger.Version)] = version;
            ledger.IncrementVersion();
            try
            {
                _ctx.Ledgers.Update(ledger);
                await _ctx.SaveChangesAsync(ct);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new ConcurrencyConflictException(
                    "An attempt was made to update ledger based on outdated data. Reload the ledger", ex);
            }
        }
    }
}
