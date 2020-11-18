using Calca.Domain.Accounting;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Calca.Domain.UnitTests
{
    public class LedgerTests
    {
        private static readonly Member _m1 = new Member(1, "ken");
        private static readonly Member _m2 = new Member(2, "mark");
        private static readonly Member _m3 = new Member(3, "katy");
        private static readonly Member _m4 = new Member(4, "alice");

        [Fact]
        public void AddOperations_ShouldBeAdded()
        {
            var operations = new List<Operation>()
            {
                new Operation(DateTime.UtcNow, new[] { _m1 }, new[] { _m2 }, 10m),
                new Operation(DateTime.UtcNow, new[] { _m2 }, new[] { _m4 }, 20m),
            };

            var sut = CreateSut();
            operations.ForEach(op => sut.AddOperation(op));
            sut.Operations.Should().BeEquivalentTo(operations);
        }

        [Theory]
        [MemberData(nameof(GetBalanceTestCases))]
        public void AddOperations_CalculateBalance(string testCaseName, List<OperationOrCancellation> operations, List<BalanceItem> expectedBalanceItems)
        {
            var sut = CreateSut();
            operations.ForEach(op => op.AddTo(sut));

            var report = sut.GetBalanceReport();
            report.Items.Should().BeEquivalentTo(expectedBalanceItems, because: testCaseName);
        }

        public static IEnumerable<object[]> GetBalanceTestCases()
        {
            yield return new object[]
            {
                "single members pay for other single members",
                new List<OperationOrCancellation>()
                {
                    GetOperation(new[] { _m1 }, new[] { _m2 }, 10m),
                    GetOperation(new[] { _m2 }, new[] { _m3 }, 10m)
                },
                new List<BalanceItem>()
                {
                    new BalanceItem(_m1, 10m),
                    new BalanceItem(_m2, 0m),
                    new BalanceItem(_m3, -10m),
                    new BalanceItem(_m4, 0m)
                }
            };

            yield return new object[]
            {
                "pair of members pay for another pair of members",
                new List<OperationOrCancellation>()
                {
                    GetOperation(new[] { _m1, _m2 }, new[] { _m3, _m4 }, 10m),
                    GetOperation(new[] { _m1, _m2 }, new[] { _m3, _m4 }, 10m),
                },
                new List<BalanceItem>()
                {
                    new BalanceItem(_m1, 10m),
                    new BalanceItem(_m2, 10m),
                    new BalanceItem(_m3, -10m),
                    new BalanceItem(_m4, -10m)
                }
            };

            yield return new object[]
            {
                "pair of members pays for everyone",
                new List<OperationOrCancellation>()
                {
                    GetOperation(new[] { _m1, _m2 }, new[] { _m1, _m2, _m3, _m4 }, 10m),
                },
                new List<BalanceItem>()
                {
                    new BalanceItem(_m1, 2.5m),
                    new BalanceItem(_m2, 2.5m),
                    new BalanceItem(_m3, -2.5m),
                    new BalanceItem(_m4, -2.5m)
                }
            };

            yield return new object[]
            {
                "all other members pay for someone",
                new List<OperationOrCancellation>()
                {
                    GetOperation(new[] { _m1, _m2, _m3 }, new[] { _m4 }, 15m),
                },
                new List<BalanceItem>()
                {
                    new BalanceItem(_m1, 5m),
                    new BalanceItem(_m2, 5m),
                    new BalanceItem(_m3, 5m),
                    new BalanceItem(_m4, -15m)
                }
            };

            yield return new object[]
            {
                "all other members pay for someone, but it is cancelled later",
                new List<OperationOrCancellation>()
                {
                    GetOperation(new[] { _m1, _m2, _m3 }, new[] { _m4 }, 15m, 1),
                    GetCancellation(1)
                },
                new List<BalanceItem>()
                {
                    new BalanceItem(_m1, 0m),
                    new BalanceItem(_m2, 0m),
                    new BalanceItem(_m3, 0m),
                    new BalanceItem(_m4, 0m)
                }
            };
        }

        private static OperationOrCancellation GetOperation(ICollection<Member> from, ICollection<Member> to, decimal amount, long id = 0)
        {
            var op = new Operation(DateTime.UtcNow, from, to, amount, id);
            return new OperationOrCancellation() { Operation = op };
        }

        private static OperationOrCancellation GetCancellation(long operationId)
        {
            return new OperationOrCancellation() { CancelledOperationId = operationId, CancelledAt = DateTime.UtcNow };
        }

        private static Ledger CreateSut()
        {
            return new Ledger(new[] { _m1, _m2, _m3, _m4 });
        }

        public class OperationOrCancellation
        {
            public Operation Operation { get; set; }

            public long? CancelledOperationId { get; set; }

            public DateTime? CancelledAt { get; set; }

            public void AddTo(Ledger ledger)
            {
                if (Operation != null)
                    ledger.AddOperation(Operation);
                else
                    ledger.CancelOperation(CancelledAt.Value, CancelledOperationId.Value);
            }
        }
    }
}
