namespace Calca.Domain.Accounting
{
    public class CancelOperationResult
    {
        public long CancellationOperationId { get; set; }

        public long LedgerVersion { get; set; }
    }
}