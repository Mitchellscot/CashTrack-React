using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CashTrack.Data.Entities;
using BCryptNet = BCrypt.Net.BCrypt;


namespace CashTrack.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> users { get; set; }
        public DbSet<Expenses> expenses { get; set; }
        public DbSet<Incomes> incomes { get; set; }
        public DbSet<ExpenseMainCategories> expense_main_categories { get;}
        public DbSet<ExpenseSubCategories> expense_sub_categories { get; set; }
        public DbSet<Merchants> merchants { get; set; }
        public DbSet<Tag> tags { get; set; }
        public DbSet<IncomeSources> income_sources { get; set; }
        public DbSet<IncomeCategories> income_categories { get; set; }

        private IConfiguration _config;

        public AppDbContext(DbContextOptions options, IConfiguration config) : base(options)
        {
            this._config = config;
        }

        //seeds the database with two users
        protected override void OnModelCreating(ModelBuilder mb)
        {
            User firstUser = new User()
            {
                Id = 1,
                FirstName = "Mitchell",
                LastName = "Scott",
                Email = "Mitchellscott@me.com",
                PasswordHash = BCryptNet.HashPassword("password"),
            };

            User secondUser = new User()
            {
                Id = 2,
                FirstName = "Sarah",
                LastName = "Scott",
                Email = "Sarahlscott@me.com",
                PasswordHash = BCryptNet.HashPassword("password"),
            };

            var userArray = new User[2]{
                firstUser, secondUser
            };

            mb.Entity<User>().HasData(userArray);
    //        modelBuilder.Entity<User>()
    //.HasMany(x => x.Roles)
    //.WithMany(x => x.Users)
    //.Map(x => x.ToTable("UserRole", "SIGNUM"));

        }

    }
}