using System;
using System.Collections.Generic;
using System.Linq;

namespace Calca.Domain.Accounting
{
    public class Ledger
    {
        public long Id { get; }
        public long Version { get; }
        public ICollection<Member> Members { get; }
        public ICollection<Operation> Operations { get; }

        public Ledger(ICollection<Member> members, ICollection<Operation> operations = null, long id = 0, long version = 0)
        {
            // TODO: check members collection

            Id = id;
            Version = version;
            Members = members;
            Operations = operations ?? new List<Operation>();
        }

        public void AddOperation(Operation op)
        {
            // TODO: check all members in operation
            Operations.Add(op);
        }

        public void CancelOperation(DateTime dateTime, long id)
        {
            var op = Operations.FirstOrDefault(o => o.Id == id);
            if (op == null)
            {
                throw new InvalidOperationException($"Operation {id} was not found in the ledger");
            }
            var cancelOp = op.CreateCancelOperation(dateTime);
            Operations.Add(cancelOp);
        }

        public BalanceReport GetBalanceReport()
        {
            // TODO: can be cached

            var subreports = Operations.SelectMany(GetBalanceChanges).GroupBy(op => op.Member.UserId);
            var balanceItems = subreports.Select(ReduceSubreport).ToList();
            foreach (var member in Members)
            {
                if (!balanceItems.Any(i => i.Member.Id == member.UserId))
                    balanceItems.Add(new BalanceItem(member, 0m));
            }
            balanceItems = balanceItems.OrderBy(i => i.Member.Name).ToList();
            return new BalanceReport(balanceItems);
        }

        private IEnumerable<BalanceItem> GetBalanceChanges(Operation op)
        {
            var avgContribution = op.Amount / op.From.Count;
            var avgConsumption = op.Amount / op.To.Count;

            foreach (var member in op.From)
                yield return new BalanceItem(member, avgContribution);

            foreach (var member in op.To)
                yield return new BalanceItem(member, -avgConsumption);
        }

        private BalanceItem ReduceSubreport(IEnumerable<BalanceItem> items)
        {
            var member = items.First().Member;
            return new BalanceItem(member, items.Sum(x => x.Balance));
        }
    }
}
