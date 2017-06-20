using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AviationManagement.Models;
using AviationManagement.Models.Manager;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Swagger;

namespace AviationManagement
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // inject db
            var connectionString = Configuration.GetConnectionString("MyConnection");
            var dbName = Environment.GetEnvironmentVariable("DB_DATABASE");
            var dbPasswd = Environment.GetEnvironmentVariable("DB_PASSWORD");
            var connection = String.Format(connectionString, dbPasswd, dbName);
            services.AddDbContext<WebAPIDbContext>(con =>
            {
                con.UseMySql(connection);
            });

            IPAddress[] IPs = Dns.GetHostAddressesAsync("redis").Result;
            // inject cache
            services.AddDistributedRedisCache(options =>
            {
                options.InstanceName = "Flight";
                options.Configuration = IPs[0].ToString();
            });

            services.AddTransient<ITokenManager, RedisTokenManager>();

            // add api doc
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

            services.AddCors(options => options.AddPolicy("flight",
                p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));

            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, WebAPIDbContext context)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseStaticFiles();

            app.UseCors("flight");

            app.UseMvc();

            DbInitializer.Initialize(context);
        }
    }
}
