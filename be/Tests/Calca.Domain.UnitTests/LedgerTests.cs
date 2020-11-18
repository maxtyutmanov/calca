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
        public void AddOperation_ShouldBeAdded()
        {
            var now = DateTime.UtcNow;

            var sut = CreateSut();
            sut.AddOperation(new Operation(now, new[] { _m1 }, new[] { _m2 }, 10m));
            sut.Operations.Should().BeEquivalentTo(new Operation(now, new[] { _m1 }, new[] { _m2 }, 10m));
        }

        [Theory]
        [MemberData("GetSimpleBalanceTestCases")]
        public void AddOperation_CalcBalance(List<Operation> operations, List<BalanceItem> expectedBalanceItems)
        {
            var sut = CreateSut();
            operations.ForEach(op => sut.AddOperation(op));

            var report = sut.GetBalanceReport();
            report.Items.Should().BeEquivalentTo(expectedBalanceItems);
        }

        public static IEnumerable<object[]> GetSimpleBalanceTestCases()
        {
            yield return new object[]
            {
                new List<Operation>()
                {
                    new Operation(DateTime.UtcNow, new[] { _m1 }, new[] { _m2 }, 10m),
                    new Operation(DateTime.UtcNow, new[] { _m2 }, new[] { _m3 }, 10m)
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
                new List<Operation>()
                {
                    new Operation(DateTime.UtcNow, new[] { _m1, _m2 }, new[] { _m3, _m4 }, 10m),
                    new Operation(DateTime.UtcNow, new[] { _m3, _m4 }, new[] { _m1, _m2 }, 10m)
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

        private Ledger CreateSut()
        {
            return new Ledger(new[] { _m1, _m2, _m3, _m4 });
        }
    }
}
