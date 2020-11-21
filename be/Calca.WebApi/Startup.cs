using Calca.Domain;
using Calca.Domain.Accounting;
using Calca.Infrastructure;
using Calca.Infrastructure.Context;
using Calca.Infrastructure.Repo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Calca.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore().AddApiExplorer().AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Shared Ledger Service API",
                    Version = "v1",
                    Description = "Shared expences accounting service",
                });
            });
            services.AddDbContext<CalcaDbContext>(o => o.UseSqlServer(Configuration["ConnectionStrings:Main"]));
            RegisterRepositories(services);
            RegisterServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            BootstrapDatabase(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Shared Ledger Service API V1");
            });
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private IServiceCollection RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<ILedgerRepository, LedgerRepository>();
            services.AddScoped<ILedgerOperationRepository, LedgerOperationRepository>();
            return services;
        }

        private IServiceCollection RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAccountingService, AccountingService>();
            return services;
        }

        private void BootstrapDatabase(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            using var ctx = scope.ServiceProvider.GetRequiredService<CalcaDbContext>();
            ctx.Database.Migrate();
        }
    }
}
