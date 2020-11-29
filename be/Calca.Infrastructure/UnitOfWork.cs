using Calca.Domain;
using Calca.Domain.Accounting;
using Calca.Infrastructure.Context;
using Calca.Infrastructure.Errors;
using Calca.Infrastructure.Repo;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
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
            : this(ctx, IsolationLevel.ReadCommitted)
        {
        }

        public UnitOfWork(CalcaDbContext ctx, IsolationLevel isoLevel)
        {
            _tran = ctx.Database.BeginTransaction(isoLevel);
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
