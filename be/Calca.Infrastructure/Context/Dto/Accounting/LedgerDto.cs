using System;
using System.Collections.Generic;
using System.Text;

namespace Calca.Infrastructure.Context.Dto.Accounting
{
    public class LedgerDto
    {
        public long Id { get; set; }
        public long Version { get; set; }
        public List<MemberDto> Members { get; set; }
        public List<OperationDto> Operations { get; set; }
    }
}
