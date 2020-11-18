using Calca.Domain.Accounting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Calca.Domain.Accounting
{
    public class Operation
    {
        public long Id { get; set; }
        public DateTime DateTime { get; }
        public ICollection<Member> From { get; }
        public ICollection<Member> To { get; }
        public decimal Amount { get; }

        public Operation(DateTime dateTime, ICollection<Member> from, ICollection<Member> to, decimal amount, long id = 0L)
        {
            DateTime = dateTime;
            From = from;
            To = to;
            Amount = amount;
            Id = id;
        }

        public Operation CreateCancelOperation(DateTime dateTime)
        {
            return new Operation(dateTime, To, From, Amount);
        }
    }
}
