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
        public DbSet<ExpenseMainCategories> ExpenseMainCategories { get; set; }
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
                id = 1,
                first_name = "Mitchell",
                last_name = "Scott",
                email = "Mitchellscott@me.com",
                password_hash = BCryptNet.HashPassword("password"),
            };

            User secondUser = new User()
            {
                id = 2,
                first_name = "Sarah",
                last_name = "Scott",
                email = "Sarahlscott@me.com",
                password_hash = BCryptNet.HashPassword("password"),
            };

            var userArray = new User[2]{
                firstUser, secondUser
            };

            mb.Entity<User>().HasData(userArray);

            mb.Entity<ExpenseTags>().HasKey(et => new { et.expense_id, et.tag_id });

            mb.Entity<ExpenseTags>()
                .HasOne<Expenses>(et => et.expense)
                .WithMany(e => e.expense_tags)
                .HasForeignKey(et => et.expense_id);
            mb.Entity<ExpenseTags>()
                .HasOne<Tags>(et => et.tag)
                .WithMany(e => e.expense_tags)
                .HasForeignKey(et => et.tag_id);
        }
    }
}