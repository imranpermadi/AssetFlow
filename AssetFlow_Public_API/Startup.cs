using FAMS_Models.Utilities;
using FAMS_Public_API.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VueCliMiddleware;

namespace FAMS_Public_API
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
            services.AddControllers();
            //services.AddSpaStaticFiles(configuration =>
            //{
            //    configuration.RootPath = "ClientApp";
            //});

            //services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            //{
            //    builder.AllowAnyOrigin()
            //           .AllowAnyMethod()
            //           .AllowAnyHeader();
            //}));

            services.AddCors();

            services
                .AddMvc()
                .AddNewtonsoftJson(o =>
                {
                    o.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    o.SerializerSettings.ContractResolver = new DefaultContractResolver();
                    o.SerializerSettings.Converters.Add(new MyDateTimeConverter());
                    o.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            //app.UseSpaStaticFiles();
            app.UseAuthorization();
            app.UseAuthentication();

            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();

            app.UseCors(builder => builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .SetIsOriginAllowed((host) => true)
                        .AllowCredentials()
                        .WithExposedHeaders("Content-Disposition")
                    );

            app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            //app.UseSpa(spa =>
            //{
            //    if (env.IsDevelopment())
            //        spa.Options.SourcePath = "ClientApp/";
            //    else
            //        spa.Options.SourcePath = "dist";

            //    if (env.IsDevelopment())
            //    {
            //        //spa.UseVueCli(npmScript: "serve");
            //    }

            //});
        }
    }

    static class Configuration
    {
        public static IConfiguration AppSettings { get; }
        static Configuration()
        {
            AppSettings = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
        }
    }

}
