using Calca.Domain.Accounting;
using System.Collections.Generic;

namespace Calca.Infrastructure.Context.Dto.Accounting
{
    public class MemberDto
    {
        public long Id { get; set; }

        public long UserId { get; set; }

        public string Name { get; set; }

        public long LedgerId { get; set; }

        public LedgerDto Ledger { get; set; }

        public List<OperationMemberDto> Operations { get; set; }

        public static Member FromDto(MemberDto dto)
        {
            return new Member(dto.UserId, dto.Name);
        }

        public static MemberDto ToDto(Member model, LedgerDto parent)
        {
            return new MemberDto()
            {
                Ledger = parent,
                LedgerId = parent?.Id ?? 0,
                Name = model.Name,
                UserId = model.UserId
            };
        }
    }
}
