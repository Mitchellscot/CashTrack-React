using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CashTrack.Helpers;
using Microsoft.EntityFrameworkCore;
using CashTrack.Data;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using CashTrack.Repositories.ExpenseRepository;
using CashTrack.Repositories.UserRepository;
using CashTrack.Repositories.TagRepository;
using CashTrack.Services.AuthenticationServices;
using Microsoft.Extensions.Logging;
using CashTrack.Repositories.MerchantRepository;

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
            string connectionString =Configuration.GetConnectionString("DefaultConnection");
            Console.WriteLine($"Using connection string: {connectionString}");

            services.AddDbContext<AppDbContext>(options => {
                options.UseNpgsql(connectionString);
                options.EnableSensitiveDataLogging(true);
            });

            //for ef core logging
            services.AddLogging(loggingBuilder => {
                loggingBuilder.AddConsole()
                    .AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Information);
                loggingBuilder.AddDebug();
            });

            //needed for webpack proxy - remove in prod
            services.AddCors();

            services.AddAutoMapper(typeof(Startup));

            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(typeof(CustomValidationFilter));
            })
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<IMerchantRepository, MerchantRepository>();
            services.AddScoped<ITagRepository, TagRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
           {
               configuration.RootPath = "./ClientApp/build";
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

               if (env.IsDevelopment() || env.IsEnvironment("Test"))
               {
                   spa.UseProxyToSpaDevelopmentServer("http://localhost:3000");
                   spa.UseReactDevelopmentServer(npmScript: "start");
               }
           });
        }
    }
}
