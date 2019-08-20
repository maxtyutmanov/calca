using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Calca.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Calca
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string storagePath;
            if (!string.IsNullOrEmpty(Configuration["StoragePath"]))
            {
                storagePath = Configuration["StoragePath"];
            }
            else
            {
                storagePath = Path.Join(Directory.GetCurrentDirectory(), "store");
            }
            
            services.AddSingleton(new FileStorage(storagePath));
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            string allowedOrigin;
            if (!string.IsNullOrEmpty(Configuration["CorsOrigin"]))
            {
                allowedOrigin = Configuration["CorsOrigin"];
            }
            else
            {
                allowedOrigin = "http://localhost:3000";
            }

            app.UseCors(policy => policy.WithOrigins(allowedOrigin).AllowAnyHeader().AllowAnyMethod().Build());
            app.UseMvc();
        }
    }
}
