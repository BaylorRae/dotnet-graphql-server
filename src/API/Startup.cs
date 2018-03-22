using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using API.middleware;
using Data;
using GraphQL;
using GraphQL.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Schema;

namespace API
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BicycleShopContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("BicycleShop"));
            });

            services.AddTransient<IDependencyResolver>(servicesProvider =>
            {
                return new FuncDependencyResolver(servicesProvider.GetService);
            });

            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddSingleton<IDocumentWriter, DocumentWriter>();

            services.AddTransient<QueryType>();
            services.AddTransient<BicycleShopSchema>();

            return services.BuildServiceProvider();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

#if DEBUG
            app.UseMiddleware<SeedDataMiddleware>();
#endif
            
            app.UseGraphQLMiddleware<BicycleShopSchema>();

            app.UseFileServer(new FileServerOptions
            {
                RequestPath = "/playground",
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "playground"
                    )
                )
            });
            
            app.Run(async (context) => { await context.Response.WriteAsync("Hello World!"); });
        }
    }
}