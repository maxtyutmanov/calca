using Calca.Domain.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calca.Infrastructure.Context.Dto.Accounting
{
    public class OperationDto
    {
        public long Id { get; set; }
        public DateTime DateTime { get; set; }
        public List<OperationMemberDto> Members { get; set; }
        public decimal Amount { get; set; }

        public long LedgerId { get; set; }
        public LedgerDto Ledger { get; set; }

        public static Operation FromDto(OperationDto dto, ICollection<Member> allMembers)
        {
            var from = dto.Members
                .Where(m => m.Side == OperationSide.Creditor)
                .Select(m => allMembers.First(mm => mm.Id == m.MemberId))
                .ToList();

            var to = dto.Members
                .Where(m => m.Side == OperationSide.Debtor)
                .Select(m => allMembers.First(mm => mm.Id == m.MemberId))
                .ToList();

            return new Operation(dto.DateTime, from, to, dto.Amount, dto.Id);
        }

        public static OperationDto ToDto(Operation model, LedgerDto parent)
        {
            var creditors = model.From.Select(m => new OperationMemberDto()
            {
                MemberId = m.Id,
                OperationId = model.Id,
                Side = OperationSide.Creditor
            });

            var debtors = model.To.Select(m => new OperationMemberDto()
            {
                MemberId = m.Id,
                OperationId = model.Id,
                Side = OperationSide.Debtor
            });

            return new OperationDto()
            {
                Id = model.Id,
                Amount = model.Amount,
                DateTime = model.DateTime,
                Members = creditors.Concat(debtors).ToList(),
                Ledger = parent,
                LedgerId = parent?.Id ?? 0
            };
        }
    }
}
