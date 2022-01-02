using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CashTrack.Data.Entities;
using CashTrack.Data.CsvFiles;
using AutoMapper;
using BCryptNet = BCrypt.Net.BCrypt;

namespace CashTrack.Data
{
    public class AppDbContext : DbContext
    {
        const string CSV_FILES = "../ct-data/csv/";
        public DbSet<Users> Users { get; set; }
        public DbSet<Expenses> Expenses { get; set; }
        public DbSet<Incomes> Incomes { get; set; }
        public DbSet<ExpenseMainCategories> ExpenseMainCategories { get; set; }
        public DbSet<ExpenseSubCategories> ExpenseSubCategories { get; set; }
        public DbSet<Merchants> Merchants { get; set; }
        public DbSet<Tags> Tags { get; set; }
        public DbSet<IncomeSources> IncomeSources { get; set; }
        public DbSet<IncomeCategories> IncomeCategories { get; set; }
        public DbSet<ExpenseToReview> ExpensesToReview { get; set; }
        public DbSet<IncomeToReview> IncomeToReview { get; set; }

        private IConfiguration _config;

        public AppDbContext(DbContextOptions options, IConfiguration config) : base(options)
        {
            this._config = config;
        }

        protected override void OnModelCreating(ModelBuilder mb)
        {

            mb.Entity<ExpenseTags>().HasKey(et => new { et.expense_id, et.tag_id });

            mb.Entity<ExpenseTags>()
                .HasOne<Expenses>(et => et.expense)
                .WithMany(e => e.expense_tags)
                .HasForeignKey(et => et.expense_id);
            mb.Entity<ExpenseTags>()
                .HasOne<Tags>(et => et.tag)
                .WithMany(e => e.expense_tags)
                .HasForeignKey(et => et.tag_id);

            mb.Entity<ExpenseMainCategories>().HasData(CsvParser.ProcessMainCategoryFile(CSV_FILES + "expense-main-categories.csv"));
            mb.Entity<ExpenseSubCategories>().HasData(CsvParser.ProcessSubCategoryFile(CSV_FILES + "expense-sub-categories.csv"));
            mb.Entity<Merchants>().HasData(CsvParser.ProcessMerchantFile(CSV_FILES + "merchants.csv"));
            mb.Entity<Expenses>().HasData(CsvParser.ProcessExpenseFile(CSV_FILES + "expenses.csv"));
            mb.Entity<IncomeCategories>().HasData(CsvParser.ProcessIncomeCategoryFile(CSV_FILES + "income-categories.csv"));
            mb.Entity<IncomeSources>().HasData(CsvParser.ProcessIncomeSourceFile(CSV_FILES + "income-sources.csv"));
            mb.Entity<Incomes>().HasData(CsvParser.ProcessIncomeFile(CSV_FILES + "incomes.csv"));
            mb.Entity<Users>().HasData(CsvParser.ProcessUserFile(CSV_FILES + "users.csv"));


        }
    }
}