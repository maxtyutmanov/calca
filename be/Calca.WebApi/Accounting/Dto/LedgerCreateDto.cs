using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calca.WebApi.Accounting.Dto
{
    public class LedgerCreateDto
    {
        public string Name { get; set; }
        public List<LedgerMemberDto> Members { get; set; }
    }
}
