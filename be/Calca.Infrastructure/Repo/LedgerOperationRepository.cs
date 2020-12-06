using Calca.Domain.Accounting;
using Calca.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Calca.Infrastructure.Repo
{
    public class LedgerOperationRepository : ILedgerOperationRepository
    {
        private readonly CalcaDbContext _ctx;

        public LedgerOperationRepository(CalcaDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<long> AddOperation(LedgerOperation operation, CancellationToken ct)
        {
            _ctx.LedgerOperations.Add(operation);
            await _ctx.SaveChangesAsync(ct);
            return operation.Id;
        }

        public async Task<bool> ExistForUsersInLedger(long ledgerId, IReadOnlyList<long> userIds, CancellationToken ct)
        {
            var query =
                from op in _ctx.LedgerOperations.Include(x => x.Members)
                where op.LedgerId == ledgerId && op.Members.Any(m => userIds.Contains(m.UserId))
                select op;

            return await query.AnyAsync(ct);
        }

        public Task<LedgerOperation> GetById(long operationId, CancellationToken ct)
        {
            return _ctx.LedgerOperations
                .Include(x => x.Members)
                .FirstOrDefaultAsync(x => x.Id == operationId);
        }

        public async Task<IReadOnlyList<LedgerOperation>> GetByLedger(long ledgerId, CancellationToken ct)
        {
            var operations = await _ctx.LedgerOperations
                .AsNoTracking()
                .Include(x => x.Members)
                .Where(op => op.LedgerId == ledgerId)
                .ToListAsync(ct);

            return operations;
        }
    }
}
