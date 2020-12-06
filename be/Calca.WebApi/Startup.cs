using Calca.Domain;
using Calca.Domain.Accounting;
using Calca.Domain.Users;
using Calca.Infrastructure;
using Calca.Infrastructure.Context;
using Calca.Infrastructure.Repo;
using Calca.WebApi.Auth;
using Calca.WebApi.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

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
                o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
            });
            services.AddSwaggerGen(options =>
            {
                var loginUrl = "/auth/external-login?provider=Google&returnUrl=/swagger";
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Shared Ledger Service API",
                    Version = "v1",
                    Description = $"Shared expenses accounting service. <a href=\"{loginUrl}\">Login</a>",
                });
            });
            services.AddDbContext<CalcaDbContext>(o => o.UseSqlServer(Configuration["ConnectionStrings:Main"]));
            RegisterSystemServices(services);
            RegisterRepositories(services);
            RegisterDomainServices(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Shared Ledger Service API V1");
            });
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private IServiceCollection RegisterSystemServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddCustomizedAuthentication(Configuration);
            services.AddCustomizedAuthorization();
            services.AddScoped<ISecurityContext, SecurityContext>();
            services.AddSingleton<ISystemClock, SystemClock>();
            services.AddScoped<IDtoMapper, DtoMapper>();
            services.AddScoped(typeof(DateTimeUtcNowResolver<,>));
            services.AddScoped(typeof(CurrentUserIdResolver<,>));
            return services;
        }

        private IServiceCollection RegisterRepositories(IServiceCollection services)
        {
            services.AddScoped<ILedgerRepository, LedgerRepository>();
            services.AddScoped<ILedgerOperationRepository, LedgerOperationRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }

        private IServiceCollection RegisterDomainServices(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAccountingService, AccountingService>();
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}
