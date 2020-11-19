using Calca.Domain.Accounting;
using Calca.Infrastructure.Context;
using Calca.Infrastructure.Context.Dto.Accounting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
            // TODO: beware of cartesian explosion
            var ledgerDto = await _ctx.Ledgers
                .Include(x => x.Members)
                .Include(x => x.Operations)
                .ThenInclude(x => x.Members)
                .FirstOrDefaultAsync(x => x.Id == id, ct);

            if (ledgerDto == null)
                return null;

            var allMembers = ledgerDto.Members.Select(MemberDto.FromDto).ToList();

            return new Ledger(
                allMembers,
                ledgerDto.Operations.Select(o => OperationDto.FromDto(o, allMembers)).ToList(),
                ledgerDto.Id,
                ledgerDto.Version);
        }

        public async Task Save(Ledger ledger, CancellationToken ct)
        {
            var ledgerDto = new LedgerDto()
            {
                Id = ledger.Id,
                Version = ledger.Version
            };

            ledgerDto.Members = ledger.Members.Select(m => MemberDto.ToDto(m, ledgerDto)).ToList();
            ledgerDto.Operations = ledger.Operations.Select(o => OperationDto.ToDto(o, ledgerDto)).ToList();

            if (ledgerDto.Id == 0)
                _ctx.Ledgers.Add(ledgerDto);
            else
                _ctx.Ledgers.Update(ledgerDto);

            ledgerDto.Version++;

            // TODO: wrap concurrency exception
            await _ctx.SaveChangesAsync(ct);
        }
    }
}
