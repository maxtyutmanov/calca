using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Calca.Domain.Accounting
{
    public class RegisterOperationResult
    {
        public long OperationId { get; set; }

        public long LedgerVersion { get; set; }
    }

    public interface IAccountingService
    {
        Task<Ledger> GetLedger(long id, CancellationToken ct);

        Task<long> CreateLedger(Ledger ledger, CancellationToken ct);

        Task<IReadOnlyList<LedgerOperation>> GetOperations(long ledgerId, CancellationToken ct);

        Task<RegisterOperationResult> RegisterOperation(LedgerOperation operation, long ledgerVersion, CancellationToken ct);
    }

    public class AccountingService : IAccountingService
    {
        private readonly ILedgerRepository _ledgerRepo;
        private readonly ILedgerOperationRepository _operationRepo;

        public AccountingService(ILedgerRepository ledgerRepo, ILedgerOperationRepository operationRepo)
        {
            _ledgerRepo = ledgerRepo;
            _operationRepo = operationRepo;
        }

        public Task<Ledger> GetLedger(long id, CancellationToken ct)
        {
            return _ledgerRepo.GetById(id, ct);
        }

        public async Task<long> CreateLedger(Ledger ledger, CancellationToken ct)
        {
            await _ledgerRepo.Add(ledger, ct);
            return ledger.Id;
        }

        public Task<IReadOnlyList<LedgerOperation>> GetOperations(long ledgerId, CancellationToken ct)
        {
            return _operationRepo.GetByLedger(ledgerId, ct);
        }

        public async Task<RegisterOperationResult> RegisterOperation(LedgerOperation operation, long ledgerVersion, CancellationToken ct)
        {
            var ledger = await _ledgerRepo.GetById(operation.LedgerId, ct);
            if (ledger == null)
            {
                // TODO: typed
                throw new InvalidOperationException("Ledger was not found in database");
            }

            var ledgerUserIds = ledger.Members.Select(m => m.UserId);
            var operationUserIds = operation.Members.Select(x => x.UserId);
            if (operationUserIds.Except(ledgerUserIds).Any())
            {
                // TODO: typed
                throw new InvalidOperationException("All members of the operation must be in the list of ledger members");
            }

            // bump ledger version by updating it
            await _ledgerRepo.Update(ledger, ledgerVersion, ct);
            var operationId = await _operationRepo.AddOperation(operation, ct);
            return new RegisterOperationResult()
            {
                LedgerVersion = ledger.Version,
                OperationId = operationId
            };
        }
    }
}
