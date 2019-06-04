using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Spinit.AspNetCore.ReverseProxy;
using Swashbuckle.AspNetCore.Swagger;

namespace ReverseProxyExample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services
                .AddReverseProxy(options =>
                {
                    //options.AddForwardedHeaders(ForwardedHeaders.All);
                })
                .AddSwaggerGen(options =>
                {
                    //options.DescribeAllEnumsAsStrings();
                    //options.DescribeAllParametersInCamelCase();
                    //options.DescribeStringEnumsInCamelCase();

                    options.SwaggerDoc("v1", new Info { Title = "ReverseProxy Example", Version = "v1" });

                    var filePath = Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, $"{PlatformServices.Default.Application.ApplicationName}.xml");
                    if (File.Exists(filePath))
                    {
                        options.IncludeXmlComments(filePath, true);
                    }
                })
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app
                .UseSwagger()
                .UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "ReverseProxy Example");
                })
                .UseMvc()
                .UseReverseProxy(c =>
                {
                    c.MapRoute(
                        "proxy/http/{*uri}",
                        rd => new Uri($"http://{rd.Values["uri"]}", UriKind.Absolute)
                    );
                    c.MapRoute(
                        "proxy/{controller}/{*path}",
                        rd => new Uri($"/api/protected/{rd.Values["path"]}", UriKind.Relative)
                    );

                })
                ;

            app.Run(request =>
            {
                request.Response.Redirect("/swagger/index.html");
                return Task.CompletedTask;
            });
        }
    }
}
