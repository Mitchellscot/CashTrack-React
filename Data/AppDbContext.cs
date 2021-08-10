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
        public DbSet<User> Users {get; set;}
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Income> Income { get; set; }
        public DbSet<ExpenseCatagory> ExpenseCatagories { get; set; }

        private IConfiguration _config;

        public AppDbContext(DbContextOptions options, IConfiguration config) : base(options)
        {
            this._config = config;
        }

        //seeds the database with two users and a number of catagories
        protected override void OnModelCreating(ModelBuilder mb)
        {
            User firstUser = new User(){
                Id=1,
                FirstName = "Mitchell",
                LastName = "Scott",
                Email = "Mitchellscott@me.com",
                PasswordHash = BCryptNet.HashPassword("password"),
            };

            User secondUser = new User(){
                Id=2,
                FirstName = "Sarah",
                LastName = "Scott",
                Email = "Sarahscott@me.com",
                PasswordHash = BCryptNet.HashPassword("password"),
            };

            var userArray = new User[2]{
                firstUser, secondUser
            };

            var catagoryArry = createCatagories();

            mb.Entity<ExpenseCatagory>().HasData(catagoryArry);

            mb.Entity<User>().HasData(userArray);

        }

        private static ExpenseCatagory[] createCatagories()
        {
            var catagoryList = new ExpenseCatagory[70]
                {
                    new ExpenseCatagory()
                    { 
                        Id = 1,
                        Name = "AAA",
                        Catagory = ExpenseMainCatagory.Insurance,
                        InUse = false
                    },
                     new ExpenseCatagory()
                    {
                        Id = 2,
                        Name = "Airfare",
                        Catagory = ExpenseMainCatagory.Vacation

                    },
                     new ExpenseCatagory()
                    {
                        Id = 3,
                        Name = "Banjo",
                        Catagory = ExpenseMainCatagory.Hobbies,
                        InUse = false
                    },
                     new ExpenseCatagory()
                    {
                        Id = 4,
                        Name = "Books",
                        Catagory = ExpenseMainCatagory.Entertainment
                    },
                     new ExpenseCatagory()
                    {
                        Id = 5,
                        Name = "Car Insurance",
                        Catagory = ExpenseMainCatagory.Transportation
                    },
                     new ExpenseCatagory()
                    {
                        Id = 6,
                        Name = "Car Repairs",
                        Catagory = ExpenseMainCatagory.Transportation
                    },
                     new ExpenseCatagory()
                    {
                        Id = 7,
                        Name = "Car Stuff",
                        Catagory = ExpenseMainCatagory.Transportation
                    },
                     new ExpenseCatagory()
                    {
                        Id = 8,
                        Name = "Car Wash",
                        Catagory = ExpenseMainCatagory.Transportation
                    },
                     new ExpenseCatagory()
                    {
                        Id = 9,
                        Name = "Church",
                        Catagory = ExpenseMainCatagory.Giving
                    },
                     new ExpenseCatagory()
                    {
                        Id = 10,
                        Name = "Clothing (Kids)",
                        Catagory = ExpenseMainCatagory.Clothing
                    },
                     new ExpenseCatagory()
                    {
                        Id = 11,
                        Name = "Clothing (Mitch)",
                        Catagory = ExpenseMainCatagory.Clothing
                    },
                     new ExpenseCatagory()
                    {
                        Id = 12,
                        Name = "Clothing (Sarah)",
                        Catagory = ExpenseMainCatagory.Clothing
                    },
                     new ExpenseCatagory()
                    {
                        Id = 13,
                        Name = "Cycling",
                        Catagory = ExpenseMainCatagory.Hobbies
                    },
                     new ExpenseCatagory()
                    {
                        Id = 14,
                        Name = "Dentist",
                        Catagory = ExpenseMainCatagory.Health
                    },
                     new ExpenseCatagory()
                    {
                        Id = 15,
                        Name = "Diapers",
                        Catagory = ExpenseMainCatagory.Kids
                    },
                     new ExpenseCatagory()
                    {
                        Id = 16,
                        Name = "Dining Out",
                        Catagory = ExpenseMainCatagory.Food
                    },
                     new ExpenseCatagory()
                    {
                        Id = 17,
                        Name = "DMV",
                        Catagory = ExpenseMainCatagory.Transportation
                    },
                     new ExpenseCatagory()
                    {
                        Id = 18,
                        Name = "Doctor (Kids)",
                        Catagory = ExpenseMainCatagory.Health
                    },
                     new ExpenseCatagory()
                    {
                        Id = 19,
                        Name = "Doctor (Mitch)",
                        Catagory = ExpenseMainCatagory.Health
                    },
                     new ExpenseCatagory()
                    {
                        Id = 20,
                        Name = "Doctor (Sarah)",
                        Catagory = ExpenseMainCatagory.Health
                    },
                     new ExpenseCatagory()
                    {
                        Id = 21,
                        Name = "Drugs",
                        Catagory = ExpenseMainCatagory.Health
                    },
                     new ExpenseCatagory()
                    {
                        Id = 22,
                        Name = "Drums",
                        Catagory = ExpenseMainCatagory.Hobbies
                    },
                     new ExpenseCatagory()
                    {
                        Id = 23,
                        Name = "Electricity",
                        Catagory = ExpenseMainCatagory.Utilities
                    },
                     new ExpenseCatagory()
                    {
                        Id = 24,
                        Name = "Electronics",
                        Catagory = ExpenseMainCatagory.Household
                    },
                     new ExpenseCatagory()
                    {
                        Id = 25,
                        Name = "Fees",
                        Catagory = ExpenseMainCatagory.Other
                    },
                     new ExpenseCatagory()
                    {
                        Id = 26,
                        Name = "Furniture",
                        Catagory = ExpenseMainCatagory.Household
                    },
                     new ExpenseCatagory()
                    {
                        Id = 27,
                        Name = "Games",
                        Catagory = ExpenseMainCatagory.Household
                    },
                     new ExpenseCatagory()
                    {
                        Id = 28,
                        Name = "Garden",
                        Catagory = ExpenseMainCatagory.Hobbies,
                        InUse = false
                    },
                     new ExpenseCatagory()
                    {
                        Id = 29,
                        Name = "Gas",
                        Catagory = ExpenseMainCatagory.Transportation
                    },
                     new ExpenseCatagory()
                    {
                        Id = 30,
                        Name = "Gifts",
                        Catagory = ExpenseMainCatagory.Giving
                    },
                     new ExpenseCatagory()
                    {
                        Id = 31,
                        Name = "Groceries",
                        Catagory = ExpenseMainCatagory.Food
                    },
                     new ExpenseCatagory()
                    {
                        Id = 32,
                        Name = "Haircut",
                        Catagory = ExpenseMainCatagory.Other
                    },
                     new ExpenseCatagory()
                    {
                        Id = 33,
                        Name = "Hobbies",
                        Catagory = ExpenseMainCatagory.Hobbies,
                        InUse = false
                    },
                     new ExpenseCatagory()
                    {
                        Id = 34,
                        Name = "Home Repairs",
                        Catagory = ExpenseMainCatagory.Household
                    },
                     new ExpenseCatagory()
                    {
                        Id = 35,
                        Name = "Home School",
                        Catagory = ExpenseMainCatagory.Kids
                    },
                     new ExpenseCatagory()
                    {
                        Id = 36,
                        Name = "Homeowners Ins.",
                        Catagory = ExpenseMainCatagory.Mortgage
                    },
                     new ExpenseCatagory()
                    {
                        Id = 37,
                        Name = "Internet",
                        Catagory = ExpenseMainCatagory.Utilities
                    },
                     new ExpenseCatagory()
                    {
                        Id = 38,
                        Name = "Kid Related",
                        Catagory = ExpenseMainCatagory.Kids
                    },
                     new ExpenseCatagory()
                    {
                        Id = 39,
                        Name = "Kitchen & Bath",
                        Catagory = ExpenseMainCatagory.Household
                    },
                     new ExpenseCatagory()
                    {
                        Id = 40,
                        Name = "Laundry",
                        Catagory = ExpenseMainCatagory.Household,
                        InUse = false
                    },
                     new ExpenseCatagory()
                    {
                        Id = 41,
                        Name = "Life Insurance",
                        Catagory = ExpenseMainCatagory.Insurance
                    },
                     new ExpenseCatagory()
                    {
                        Id = 42,
                        Name = "Mandolin",
                        Catagory = ExpenseMainCatagory.Hobbies
                    },
                     new ExpenseCatagory()
                    {
                        Id = 43,
                        Name = "Mortgage",
                        Catagory = ExpenseMainCatagory.Mortgage
                    },
                     new ExpenseCatagory()
                    {
                        Id = 44,
                        Name = "Movies",
                        Catagory = ExpenseMainCatagory.Entertainment
                    },
                     new ExpenseCatagory()
                    {
                        Id = 45,
                        Name = "Moving Expenses",
                        Catagory = ExpenseMainCatagory.Other,
                        InUse = false
                    },
                     new ExpenseCatagory()
                    {
                        Id = 46,
                        Name = "Music",
                        Catagory = ExpenseMainCatagory.Entertainment
                    },
                     new ExpenseCatagory()
                    {
                        Id = 47,
                        Name = "Natural Gas",
                        Catagory = ExpenseMainCatagory.Utilities
                    },
                     new ExpenseCatagory()
                    {
                        Id = 48,
                        Name = "Office Supplies",
                        Catagory = ExpenseMainCatagory.Household
                    },
                     new ExpenseCatagory()
                    {
                        Id = 49,
                        Name = "Other",
                        Catagory = ExpenseMainCatagory.Other
                    },
                     new ExpenseCatagory()
                    {
                        Id = 50,
                        Name = "Outdoor",
                        Catagory = ExpenseMainCatagory.Hobbies
                    },
                     new ExpenseCatagory()
                    {
                        Id = 51,
                        Name = "Pet",
                        Catagory = ExpenseMainCatagory.Hobbies,
                        InUse = false
                    },
                     new ExpenseCatagory()
                    {
                        Id = 52,
                        Name = "Phone",
                        Catagory = ExpenseMainCatagory.Utilities
                    },
                     new ExpenseCatagory()
                    {
                        Id = 53,
                        Name = "Property Taxes",
                        Catagory = ExpenseMainCatagory.Mortgage
                    },
                     new ExpenseCatagory()
                    {
                        Id = 54,
                        Name = "Reimbursement",
                        Catagory = ExpenseMainCatagory.Reimbursement
                    },
                     new ExpenseCatagory()
                    {
                        Id = 55,
                        Name = "Rent",
                        Catagory = ExpenseMainCatagory.Mortgage,
                        InUse = false
                    },
                     new ExpenseCatagory()
                    {
                        Id = 56,
                        Name = "Running",
                        Catagory = ExpenseMainCatagory.Hobbies
                    },
                     new ExpenseCatagory()
                    {
                        Id = 57,
                        Name = "School",
                        Catagory = ExpenseMainCatagory.Other,
                        InUse = false
                    },
                     new ExpenseCatagory()
                    {
                        Id = 58,
                        Name = "Seasonal",
                        Catagory = ExpenseMainCatagory.Other
                    },
                     new ExpenseCatagory()
                    {
                        Id = 59,
                        Name = "Sewing",
                        Catagory = ExpenseMainCatagory.Hobbies,
                        InUse = false
                    },
                     new ExpenseCatagory()
                    {
                        Id = 60,
                        Name = "Shipping",
                        Catagory = ExpenseMainCatagory.Other
                    },
                     new ExpenseCatagory()
                    {
                        Id = 61,
                        Name = "Shooting",
                        Catagory = ExpenseMainCatagory.Hobbies,
                        InUse = false
                    },
                     new ExpenseCatagory()
                    {
                        Id = 62,
                        Name = "Software",
                        Catagory = ExpenseMainCatagory.Household
                    },
                     new ExpenseCatagory()
                    {
                        Id = 63,
                        Name = "Swimming",
                        Catagory = ExpenseMainCatagory.Hobbies
                    },
                     new ExpenseCatagory()
                    {
                        Id = 64,
                        Name = "Taxes",
                        Catagory = ExpenseMainCatagory.Other,
                        InUse = false
                    },
                     new ExpenseCatagory()
                    {
                        Id = 65,
                        Name = "Toiletries",
                        Catagory = ExpenseMainCatagory.Household
                    },
                     new ExpenseCatagory()
                    {
                        Id = 66,
                        Name = "Toys",
                        Catagory = ExpenseMainCatagory.Kids
                    },
                     new ExpenseCatagory()
                    {
                        Id = 67,
                        Name = "Travel Misc",
                        Catagory = ExpenseMainCatagory.Vacation
                    },
                     new ExpenseCatagory()
                    {
                        Id = 68,
                        Name = "Water",
                        Catagory = ExpenseMainCatagory.Utilities
                    },
                     new ExpenseCatagory()
                    {
                        Id = 69,
                        Name = "Weight Training",
                        Catagory = ExpenseMainCatagory.Hobbies
                    },
                     new ExpenseCatagory()
                    {
                        Id = 70,
                        Name = "Yard",
                        Catagory = ExpenseMainCatagory.Household
                    },
                };
            return catagoryList;
        }
        
    }
}