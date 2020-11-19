using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calca.WebApi.Accounting.Dto
{
    public class LedgerApiDto
    {
        public long Id { get; set; }
        public long Version { get; set; }
        public List<MemberApiDto> Members { get; set; }
        public List<OperationApiDto> Operations { get; set; }
    }
}
