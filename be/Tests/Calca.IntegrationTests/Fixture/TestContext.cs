using Calca.Domain.Users;
using Calca.Infrastructure.Context;
using Calca.WebApi;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Calca.IntegrationTests.Fixture
{
    public class TestContext : IAsyncDisposable
    {
        private readonly WebApplicationFactory<Startup> _appFactory;

        public HttpClient Client { get; }

        public TestContext(ITestOutputHelper testOutput)
        {
            _appFactory = new AppFactory<Startup>().WithWebHostBuilder(b =>
            {
                b.UseSerilog((ctx, loggerConf) =>
                {
                    loggerConf.WriteTo.TestOutput(testOutput);
                });
            });
            Client = _appFactory.CreateClient();
        }

        public async ValueTask DisposeAsync()
        {
            Client.Dispose();
            // to avoid deadlocks
            await _appFactory.Server.Host.StopAsync();
            _appFactory.Dispose();
        }

        public async Task AddTestUser(User user)
        {
            using var scope = _appFactory.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<CalcaDbContext>();
            db.Users.Add(user);
            await db.SaveChangesAsync();
        }

        public void SetFixedMomentOfTime(DateTime utcNow)
        {
            var sc = _appFactory.Services.GetRequiredService<ControlledSystemClock>();
            sc.Override(utcNow);
        }
    }
}
