using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Calca.WebApi.Accounting.Dto
{
    public class OperationApiDto
    {
        public long Id { get; set; }
        public DateTime DateTime { get; set; }
        public List<MemberApiDto> From { get; set; }
        public List<MemberApiDto> To { get; set; }
        public decimal Amount { get; set; }
    }
}
