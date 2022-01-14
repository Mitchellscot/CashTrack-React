using AutoMapper;
using CashTrack.Data;
using CashTrack.Data.Entities;
using CashTrack.Helpers;
using CashTrack.IntegrationTests;
using CashTrack.Repositories.ExpenseRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace CashTrack.Tests.Repositories
{
    public class ExpenseRepositoryTests
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ExpenseRepository> _logger;
        private readonly IOptions<AppSettings> _settings;
        private readonly IConfiguration _config;
        private readonly ITestOutputHelper _output;

        public ExpenseRepositoryTests(ITestOutputHelper output)
        {
            _mapper = Mock.Of<IMapper>();
            _logger = Mock.Of<ILogger<ExpenseRepository>>();
            _settings = Mock.Of<IOptions<AppSettings>>();
            _config = Mock.Of<IConfiguration>();
            _output = output;
        }

        [Fact]
        public void GetExpenseByIdTEST()
        {
            //OK this works BUT
            //The problem is: I have to add a hard coded URL in the appdbcontext to make this work, as it reads from a csv file
            //and it always wants to read from the debug folder... I solved the problem in the integration test project,
            //but not this one.
            //SECOND
            //I use automapper INthe repository, and honestly this might not be the best use case as it adds dependancies.
            //Mocking does not help me, as it returns nothing.
            //So in order to get this test to work, I only had it return the single entity
            //and that isn't helpful
            //So I think for this project
            //Either hardcode the path in AppDbContext and just leave it, don't add it to an options file (not sure why i did that)
            //AND move the mapping to the controller
            //AND just have the repository return entities
            //Or just do integration tests for this project.
            //THINK about all that....
            //Do you really want to have to mock out an in memory database EVERY time??

            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("cash_track_test").Options;
            using (var context = new AppDbContext(options, _config))
            {
                context.Expenses.Add(new Expenses()
                {
                    id = 1,
                    purchase_date = DateTime.UtcNow,
                    amount = (decimal)25.00,
                    category = new ExpenseSubCategories()
                    {
                        id = 1,
                        sub_category_name = "Dinner",
                        main_category = new ExpenseMainCategories()
                        {
                            id = 1,
                            main_category_name = "Food",
                            sub_categories = new List<ExpenseSubCategories>()
                        },
                        in_use = true,
                        expenses = new List<Expenses>()
                    }
                });
                context.SaveChanges();
                var expenseRepository = new ExpenseRepository(_settings, context, _logger, _mapper);

                var x = expenseRepository.GetExpenseByIdTEST(1);
                Assert.Contains("25", x.Result.amount.ToString());
            }
        }

    }
}
