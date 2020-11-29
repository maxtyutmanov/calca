using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Calca.Domain.Accounting
{
    public class BalanceSheet
    {
        private readonly Dictionary<LedgerMember, decimal> _items;
        public IReadOnlyDictionary<LedgerMember, decimal> Items => _items;

        public BalanceSheet(IReadOnlyList<LedgerMember> allLedgerMembers)
            : this(allLedgerMembers.ToDictionary(x => x, x => 0.0m))
        {
        }

        private BalanceSheet(Dictionary<LedgerMember, decimal> items)
        {
            _items = items;
        }

        public BalanceSheet AddOperations(IReadOnlyList<LedgerOperation> operations)
        {
            var newItems = new Dictionary<LedgerMember, decimal>(Items);
            foreach (var operation in operations)
            {
                var creditors = operation.Members.Where(x => x.Side == OperationSide.Creditor).ToList();
                var debtors = operation.Members.Where(x => x.Side == OperationSide.Debtor).ToList();
                var avgPlus = operation.Amount / creditors.Count;
                var avgMinus = operation.Amount / debtors.Count;

                foreach (var creditor in creditors)
                {
                    var member = GetLedgerMemberByUserId(creditor.UserId);
                    newItems[member] += avgPlus;
                }

                foreach (var debtor in debtors)
                {
                    var member = GetLedgerMemberByUserId(debtor.UserId);
                    newItems[member] -= avgMinus;
                }    
            }

            return new BalanceSheet(newItems);
        }

        private LedgerMember GetLedgerMemberByUserId(long userId)
        {
            var member = Items.Keys.FirstOrDefault(x => x.UserId == userId);
            return member ?? throw new InvalidOperationException($"User {userId} was not found in ledger members list");
        }
    }
}
