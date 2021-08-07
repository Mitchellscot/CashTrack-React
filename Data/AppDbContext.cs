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
        private IConfiguration _config;

        public AppDbContext(DbContextOptions options, IConfiguration config) : base(options)
        {
            this._config = config;
        }

        //seeds the database with two users
        protected override void OnModelCreating(ModelBuilder mb)
        {
            User firstUser = new User(){
                Id=1,
                FirstName = "Mitchell",
                LastName = "Scott",
                Email = "Mitchellscott@me.com",
                Role = Role.Manager,
                PasswordHash = BCryptNet.HashPassword("password"),
                ResetToken = ""
            };

            User secondUser = new User(){
                Id=2,
                FirstName = "Sarah",
                LastName = "Scott",
                Email = "Sarahscott@me.com",
                Role = Role.Guide,
                PasswordHash = BCryptNet.HashPassword("password"),
                ResetToken = ""
            };

            var userArray = new User[2]{
                firstUser, secondUser
            };

            mb.Entity<User>().HasData(userArray);

        }
        
    }
}