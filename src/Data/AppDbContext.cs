using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CashTrack.Data.Entities;
using AutoMapper;
using BCryptNet = BCrypt.Net.BCrypt;

namespace CashTrack.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Expenses> Expenses { get; set; }
        public DbSet<Incomes> Incomes { get; set; }
        public DbSet<ExpenseMainCategories> ExpenseMainCategories { get;}
        public DbSet<ExpenseSubCategories> ExpenseSubCategories { get; set; }
        public DbSet<Merchants> Merchants { get; set; }
        public DbSet<Tags> Tags { get; set; }
        public DbSet<IncomeSources> IncomeSources { get; set; }
        public DbSet<IncomeCategories> IncomeCategories { get; set; }

        private IConfiguration _config;

        public AppDbContext(DbContextOptions options, IConfiguration config) : base(options)
        {
            this._config = config;
        }

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

            mb.Entity<ExpenseTags>().HasKey(et => new { et.ExpenseId, et.TagId });

            mb.Entity<ExpenseTags>()
                .HasOne<Expenses>(et => et.Expense)
                .WithMany(e => e.ExpenseTags)
                .HasForeignKey(et => et.ExpenseId);
            mb.Entity<ExpenseTags>()
                .HasOne<Tags>(et => et.Tag)
                .WithMany(e => e.ExpenseTags)
                .HasForeignKey(et => et.TagId);
        }
    }
}