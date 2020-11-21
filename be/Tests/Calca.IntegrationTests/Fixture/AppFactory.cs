using Calca.Infrastructure.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Calca.Domain;

namespace Calca.IntegrationTests.Fixture
{
    public class AppFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureServices(services =>
            {
                PlugInMemoryDb(services);
                PlugControlledSystemClock(services);
            });
        }

        private void PlugControlledSystemClock(IServiceCollection services)
        {
            var descriptor = services.First(d => d.ServiceType == typeof(ISystemClock));
            services.Remove(descriptor);

            services.AddSingleton<ControlledSystemClock>();
            services.AddSingleton<ISystemClock>(p => p.GetRequiredService<ControlledSystemClock>());
        }

        private void PlugInMemoryDb(IServiceCollection services)
        {
            var descriptor = services.First(d => d.ServiceType == typeof(DbContextOptions<CalcaDbContext>));
            services.Remove(descriptor);

            services.AddDbContext<CalcaDbContext>(options =>
            {
                options.UseInMemoryDatabase("Calca.InMemory");
                options.ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });
        }
    }
}
