using System;
using System.Collections.Generic;
using System.Text;

namespace Calca.Infrastructure.Context.Dto.Accounting
{
    public class OperationMemberDto
    {
        public long MemberId { get; set; }

        public long OperationId { get; set; }

        public OperationSide Side { get; set; }

        public MemberDto Member { get; set; }

        public OperationDto Operation { get; set; }
    }
}
