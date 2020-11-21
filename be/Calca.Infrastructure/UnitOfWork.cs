using Calca.Domain;
using Calca.Domain.Accounting;
using Calca.Infrastructure.Context;
using Calca.Infrastructure.Repo;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Calca.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IDbContextTransaction _tran;
        private bool _committed;

        public UnitOfWork(CalcaDbContext ctx)
        {
            _tran = ctx.Database.BeginTransaction();
        }

        public async Task Commit(CancellationToken ct)
        {
            await _tran.CommitAsync(ct);
            _committed = true;
        }

        public void Dispose()
        {
            if (!_committed)
                _tran.Rollback();
        }
    }
}
