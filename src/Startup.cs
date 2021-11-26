using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CashTrack.Helpers;
using CashTrack.Data.Services.UserRepository;
using Microsoft.EntityFrameworkCore;
using CashTrack.Data;
using CashTrack.Services.ExpenseRepository;
using CashTrack.Services.TagRepository;
using FluentValidation.AspNetCore;
using CashTrack.Helpers.Validators;
using Microsoft.AspNetCore.Mvc;

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
            services.AddAutoMapper(typeof(Startup));

            services.AddMvc(opt =>
            {
                opt.Filters.Add(typeof(CustomValidationFilter));
            })
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddScoped<IExpenseService, ExpenseService>();
            services.AddScoped<ITagService, TagService>();
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
