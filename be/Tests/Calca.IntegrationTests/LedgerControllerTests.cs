﻿using Calca.Domain.Users;
using Calca.IntegrationTests.Fixture;
using Calca.IntegrationTests.Utils;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Calca.IntegrationTests
{
    public class LedgerControllerTests : IAsyncLifetime
    {
        private const long TestUserId1 = 1;
        private const long TestUserId2 = 2;
        private const long TestUserId3 = 3;

        private readonly TestContext _ctx;

        public LedgerControllerTests(ITestOutputHelper testOutput)
        {
            _ctx = new TestContext(testOutput);
        }

        public async Task InitializeAsync()
        {
            await _ctx.AddTestUser(new User() { Id = TestUserId1, Email = "test1@test.org", Name = "test1" });
            await _ctx.AddTestUser(new User() { Id = TestUserId2, Email = "test2@test.org", Name = "test2" });
            await _ctx.AddTestUser(new User() { Id = TestUserId3, Email = "test3@test.org", Name = "test3" });
        }

        public async Task DisposeAsync()
        {
            await _ctx.DisposeAsync();
        }

        [Fact]
        public async Task CreateNewLedger_GetItBack_ShouldBeSame()
        {
            // arrange

            var now = DateTime.UtcNow;
            _ctx.SetFixedMomentOfTime(now);

            // act

            var ledgerId = await CreateTestLedger("test ledger", new[] { TestUserId1, TestUserId2 });
            
            // assert
            
            var expectedLedger = new
            {
                name = "test ledger",
                members = new[]
                {
                    new { userId = TestUserId1 },
                    new { userId = TestUserId2 }
                },
                version = 1,
                id = 1,
                createdAt = now,
                createdBy = 0
            };

            var fetchedLedger = await _ctx.Client.GetJsonObject(expectedLedger, $"/ledgers/{ledgerId}");
            fetchedLedger.Should().BeEquivalentTo(expectedLedger);
        }

        [Fact]
        public async Task CreateNewLedger_UpdateAndGetBack_ShouldBeUpdated()
        {
            // arrange

            var now = DateTime.UtcNow;
            _ctx.SetFixedMomentOfTime(now);
            var ledgerId = await CreateTestLedger("test ledger", new[] { TestUserId1, TestUserId2 });
            var ledgerVersion = await GetLedgerVersion(ledgerId);

            // act
            
            await UpdateLedger(ledgerId, ledgerVersion, "test ledger 2", new[] { TestUserId2, TestUserId3 });

            // assert

            var expectedLedger = new
            {
                name = "test ledger 2",
                members = new[]
                {
                    new { userId = TestUserId2 },
                    new { userId = TestUserId3 }
                },
                version = ledgerVersion + 1,
                id = ledgerId,
                createdAt = now,
                createdBy = 0
            };

            var fetchedLedger = await _ctx.Client.GetJsonObject(expectedLedger, $"/ledgers/{ledgerId}");
            fetchedLedger.Should().BeEquivalentTo(expectedLedger);
        }

        [Fact]
        public async Task CreateNewOperationOnLedger_GetAllBack_ShouldBeFetched()
        {
            // arrange

            var now = DateTime.UtcNow;
            _ctx.SetFixedMomentOfTime(now);
            var ledgerId = await CreateTestLedger("test ledger", new[] { TestUserId1, TestUserId2 });
            var ledgerVersion = await GetLedgerVersion(ledgerId);

            // act

            var operation = new
            {
                description = "some test operation",
                amount = 110m,
                members = new[]
                {
                    new { userId = TestUserId1, side = "creditor" },
                    new { userId = TestUserId2, side = "debtor" }
                },
                ledgerVersion = ledgerVersion
            };

            await CreateOperation(ledgerId, operation);

            // assert

            var expectedOperations = new []
            {
                new
                {
                    id = 1L,
                    amount = 110m,
                    members = new[]
                    {
                        new { userId = TestUserId1, side = "creditor" },
                        new { userId = TestUserId2, side = "debtor" }
                    },
                    createdAt = now,
                    createdBy = 0
                }
            };

            var fetchedOperations = await _ctx.Client.GetJsonObject(expectedOperations, $"/ledgers/{ledgerId}/operations");
            fetchedOperations.Should().BeEquivalentTo(expectedOperations);
        }

        [Fact]
        public async Task CreateNewOperationOnLedger_CancelIt_ShouldAddCompensatingOperation()
        {
            // arrange

            var now = DateTime.UtcNow;
            _ctx.SetFixedMomentOfTime(now);
            var ledgerId = await CreateTestLedger("test ledger", new[] { TestUserId1, TestUserId2 });
            var ledgerVersion = await GetLedgerVersion(ledgerId);

            // act

            var operation = new
            {
                description = "some test operation",
                amount = 110m,
                members = new[]
                {
                    new { userId = TestUserId1, side = "creditor" },
                    new { userId = TestUserId2, side = "debtor" }
                },
                ledgerVersion = ledgerVersion
            };

            await CreateOperation(ledgerId, operation);
            ledgerVersion = await GetLedgerVersion(ledgerId);
            await CancelOperation(ledgerId, 1L, ledgerVersion);

            // assert

            var expectedOperations = new[]
            {
                new
                {
                    id = 1L,
                    amount = 110m,
                    members = new[]
                    {
                        new { userId = TestUserId1, side = "creditor" },
                        new { userId = TestUserId2, side = "debtor" }
                    },
                    createdAt = now,
                    createdBy = 0
                },
                new
                {
                    id = 2L,
                    amount = 110m,
                    members = new[]
                    {
                        new { userId = TestUserId2, side = "creditor" },
                        new { userId = TestUserId1, side = "debtor" }
                    },
                    createdAt = now,
                    createdBy = 0
                }
            };

            var fetchedOperations = await _ctx.Client.GetJsonObject(expectedOperations, $"/ledgers/{ledgerId}/operations");
            fetchedOperations.Should().BeEquivalentTo(expectedOperations);
        }

        [Fact]
        public async Task CreateLedger_TryAddOperationWithOldVersion_ShouldBeConflict()
        {
            // arrange

            var now = DateTime.UtcNow;
            _ctx.SetFixedMomentOfTime(now);
            var ledgerId = await CreateTestLedger("test ledger", new[] { TestUserId1, TestUserId2 });
            var ledgerVersion = await GetLedgerVersion(ledgerId);

            // act, assert

            var operation = new
            {
                description = "some test operation",
                amount = 110m,
                members = new[]
                {
                    new { userId = TestUserId1, side = "creditor" },
                    new { userId = TestUserId2, side = "debtor" }
                },
                ledgerVersion = ledgerVersion - 1
            };

            await CreateOperation(ledgerId, operation, resp =>
            {
                resp.StatusCode.Should().Be(HttpStatusCode.Conflict);
            });
        }

        private async Task CancelOperation(long ledgerId, long operationId, long ledgerVersion)
        {
            var cancellation = new
            {
                operationId = operationId,
                ledgerVersion = ledgerVersion
            };

            var resp = await _ctx.Client.PostJson($"/ledgers/{ledgerId}/cancellations", cancellation);
            resp.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        private async Task CreateOperation(long ledgerId, object operation, Action<HttpResponseMessage> handle = null)
        {
            var resp = await _ctx.Client.PostJson($"/ledgers/{ledgerId}/operations", operation);

            handle ??= r => resp.StatusCode.Should().Be(HttpStatusCode.OK);
            handle(resp);
        }

        private async Task<long> GetLedgerVersion(long ledgerId)
        {
            var responseSample = new
            {
                version = 0L
            };

            var resp = await _ctx.Client.GetJsonObject(responseSample, $"/ledgers/{ledgerId}");
            return resp.version;
        }

        private async Task<long> CreateTestLedger(string name, IEnumerable<long> memberUserIds)
        {
            var members = memberUserIds.Select(mid => new { userId = mid }).ToList();

            var ledger = new
            {
                name = name,
                members = members
            };

            var createResp = await _ctx.Client.PostJson("/ledgers", ledger);
            createResp.StatusCode.Should().Be(HttpStatusCode.Created);
            createResp.Headers.Location.Should().NotBeNull();

            var idStr = createResp.Headers.Location.Segments.LastOrDefault();
            idStr.Should().NotBeNullOrWhiteSpace();
            long.TryParse(idStr, out var id).Should().BeTrue();
            return id;
        }

        private async Task UpdateLedger(long id, long version, string name, IEnumerable<long> memberUserIds)
        {
            var members = memberUserIds.Select(mid => new { userId = mid }).ToList();

            var ledger = new
            {
                name = name,
                members = members,
                version = version
            };

            var updateResp = await _ctx.Client.PutJson($"/ledgers/{id}", ledger);
            updateResp.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
