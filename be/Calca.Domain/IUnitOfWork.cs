using Calca.Domain.Accounting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Calca.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        Task Commit(CancellationToken ct);
        ILedgerRepository GetLedgerRepository();
    }
}
