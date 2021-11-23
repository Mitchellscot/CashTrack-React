using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CashTrack.Helpers;
using CashTrack.Data.Services.Users;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using CashTrack.Data;
using AutoMapper;
using CashTrack.Data.Entities;
using CashTrack.Models.Users;
using CashTrack.Services.Expenses;

namespace CashTrack
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
            //this code is added in case I want to deploy to Heroku later:
            string DATABASE_URL = Environment.GetEnvironmentVariable("DATABASE_URL_STR");
            string connectionString = (DATABASE_URL == null ? Configuration.GetConnectionString("DefaultConnection") : DATABASE_URL);
            Console.WriteLine($"Using connection string: {connectionString}");
            //using AppDbContext with Postgres database
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(connectionString)
            );
            //needed for webpack proxy
            services.AddCors();
            services.AddControllersWithViews();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            //services.AddScoped<IExpenseService, ExpenseService>();
            services.AddScoped<IUserService, UserService>();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
           {
               configuration.RootPath = "ClientApp/build";
           });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .WithOrigins("http://localhost:3000")
                .AllowAnyMethod()
                .AllowAnyHeader());
            // custom jwt auth middleware
            app.UseMiddleware<JwtMiddleware>();
            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
           {
               spa.Options.SourcePath = "ClientApp";

               if (env.IsDevelopment())
               {
                   spa.UseProxyToSpaDevelopmentServer("http://localhost:3000");
                   spa.UseReactDevelopmentServer(npmScript: "start");
               }
           });
        }
    }
}
