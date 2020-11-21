using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calca.WebApi.Accounting.Dto
{
    public class LedgerDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public List<LedgerMemberDto> Members { get; set; }
        public long Version { get; set; }
        public long CreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
