using Calca.IntegrationTests.Fixture;
using Calca.IntegrationTests.Utils;
using FluentAssertions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Calca.IntegrationTests
{
    public class LedgerControllerTests : IAsyncDisposable
    {
        private readonly TestContext _ctx;

        public LedgerControllerTests(ITestOutputHelper testOutput)
        {
            _ctx = new TestContext(testOutput);
        }

        public async ValueTask DisposeAsync()
        {
            await _ctx.DisposeAsync();
        }

        [Fact]
        public async Task CreateNewLedger_GetItBack_ShouldBeSame()
        {
            // arrange

            var now = DateTime.UtcNow;
            _ctx.SetFixedMomentOfTime(now);

            await _ctx.AddTestUser(new Domain.Users.User() { Id = 1, Email = "test1@test.org", Name = "test1" });
            await _ctx.AddTestUser(new Domain.Users.User() { Id = 2, Email = "test2@test.org", Name = "test2" });

            var initialLedger = new
            {
                name = "test ledger",
                members = new[]
                {
                    new { userId = 1 },
                    new { userId = 2 }
                }
            };

            // act

            var createResp = await _ctx.Client.PostJson("/ledgers", initialLedger);
            createResp.StatusCode.Should().Be(HttpStatusCode.Created);
            createResp.Headers.Location.Should().NotBeNull();
            
            // assert
            
            var resourcePath = createResp.Headers.Location.AbsolutePath;
            var expectedLedger = new
            {
                name = "test ledger",
                members = new[]
                {
                    new { userId = 1 },
                    new { userId = 2 }
                },
                version = 1,
                id = 1,
                createdAt = now,
                createdBy = 0
            };

            var fetchedLedger = await _ctx.Client.GetJsonObject(expectedLedger, resourcePath);
            fetchedLedger.Should().BeEquivalentTo(expectedLedger);
        }
    }
}
