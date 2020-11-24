using System;
using System.Collections.Generic;
using System.Text;

namespace Calca.Domain.Accounting
{
    public class OperationMember
    {
        public long OperationId { get; private set; }

        public long UserId { get; private set; }

        public OperationSide Side { get; set; }

        private OperationMember() { }

        public OperationMember(long operationId, long userId, OperationSide side)
        {
            OperationId = operationId;
            UserId = userId;
            Side = side;
        }
    }
}
