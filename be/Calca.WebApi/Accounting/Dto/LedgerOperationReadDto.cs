using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calca.WebApi.Accounting.Dto
{
    public class LedgerOperationReadDto
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public List<OperationMemberDto> Members { get; set; }
        public decimal Amount { get; set; }
        public long CreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
