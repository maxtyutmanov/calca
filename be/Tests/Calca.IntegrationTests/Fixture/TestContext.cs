using Calca.Domain.Users;
using Calca.Infrastructure;
using Calca.Infrastructure.Context;
using Calca.WebApi;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Mvc.Testing.Handlers;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Calca.IntegrationTests.Fixture
{
    public class TestContext : IAsyncDisposable
    {
        private readonly CookieContainer _cookies = new CookieContainer();
        private readonly TestServer _server;
        public HttpClient Client { get; }

        public TestContext(ITestOutputHelper testOutput)
        {
            var hostBuilder = WebHost.CreateDefaultBuilder()
                .UseStartup<TestStartup>()
                .UseSerilog((ctx, loggerConf) =>
                {
                    loggerConf.WriteTo.TestOutput(testOutput);
                });
            _server = new TestServer(hostBuilder);
            var handler = new CookieContainerHandler(_cookies) { InnerHandler = _server.CreateHandler() };
            Client = new HttpClient(handler)
            {
                BaseAddress = _server.BaseAddress
            };
        }

        public async ValueTask DisposeAsync()
        {
            await DeleteDb();
            Client.Dispose();
            // to avoid deadlocks
            await _server.Host.StopAsync();
            _server.Dispose();
        }

        public async Task<long> AddTestUser(User user)
        {
            using var scope = _server.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<CalcaDbContext>();
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return user.Id;
        }

        public async Task Login(User user)
        {
            var claimsDict = new Dictionary<string, string>()
            {
                [KnownClaimTypes.UserId] = user.Id.ToString(),
                [ClaimTypes.Name] = user.Name,
                [ClaimTypes.Email] = user.Email
            };

            var claimsStr = JsonSerializer.Serialize(claimsDict);
            var content = new StringContent(claimsStr, Encoding.UTF8, "application/json");
            var loginResponse = await Client.PostAsync("/test-only/login", content);
            loginResponse.EnsureSuccessStatusCode();
        }

        public void SetFixedMomentOfTime(DateTime utcNow)
        {
            var sc = _server.Services.GetRequiredService<ControlledSystemClock>();
            sc.Override(utcNow);
        }

        private async Task DeleteDb()
        {
            using var scope = _server.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<CalcaDbContext>();
            await db.Database.EnsureDeletedAsync();
        }
    }
}
