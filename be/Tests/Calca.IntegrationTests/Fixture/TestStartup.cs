using Calca.Infrastructure.Context;
using Calca.WebApi;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Calca.IntegrationTests.Fixture
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration)
            : base(configuration)
        {
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            
            PlugInMemoryDb(services);
            PlugControlledSystemClock(services);
        }

        protected override void BeforeConfigure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            base.BeforeConfigure(app, env);

            app.Use(async (ctx, next) =>
            {
                if (ctx.Request.Path != "/test-only/login")
                {
                    await next();
                    return;
                }

                using var reader = new StreamReader(ctx.Request.Body);
                var bodyStr = await reader.ReadToEndAsync();
                var claimsDict = JsonSerializer.Deserialize<Dictionary<string, string>>(bodyStr);
                var claims = claimsDict.Select(pair => new Claim(pair.Key, pair.Value)).ToList();
                var id = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(id);
                await ctx.SignInAsync(principal);
            });
        }

        private void PlugControlledSystemClock(IServiceCollection services)
        {
            var descriptor = services.First(d => d.ServiceType == typeof(Domain.ISystemClock));
            services.Remove(descriptor);

            services.AddSingleton<ControlledSystemClock>();
            services.AddSingleton<Domain.ISystemClock>(p => p.GetRequiredService<ControlledSystemClock>());
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
