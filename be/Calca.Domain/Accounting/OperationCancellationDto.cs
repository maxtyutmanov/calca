using System;
using System.Collections.Generic;
using System.Text;

namespace Calca.Domain.Accounting
{
    public class OperationCancellationDto
    {
        public long OperationId { get; set; }

        public long LedgerVersion { get; set; }
    }
}
