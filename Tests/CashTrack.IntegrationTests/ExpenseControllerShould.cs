using CashTrack.IntegrationTests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Shouldly;
using Newtonsoft.Json;
using CashTrack.Models.ExpenseModels;
using System.Net;

namespace CashTrack.IntegrationTests
{
    public class ExpenseControllerShould : IClassFixture<TestServerFixture>
    {
        private TestServerFixture _fixture;
        private ITestOutputHelper _output;
        const string path = "/api/expense";

        public ExpenseControllerShould(TestServerFixture fixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            _output = output;
        }
        #region SingleExpense
        [Theory]
        [ExpenseIdData]
        public async void ReturnASingleExpense(string id)
        {
            var response = await _fixture.Client.GetAsync(path + "/" + id);

            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            _output.WriteLine(responseString);

            Assert.Contains($"\"id\":{id}", responseString);
        }
        [Theory]
        [InlineData(0)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public async void ErrorWithInvalidId(int id)
        {
            var response = await _fixture.Client.GetAsync(path + "/" + id);
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.BadRequest);

            var responseString = await response.Content.ReadAsStringAsync();
            _output.WriteLine(responseString);

            Assert.Contains($"No expense found with an id of {id}", responseString);
        }
        [Theory]
        [InlineData("%")]
        [InlineData("A")]
        [EmptyData]
        public async void ErrorWithInvalidInput(object input)
        {
            var response = await _fixture.Client.GetAsync(path + "/" + input);
            response.StatusCode.ShouldBe(System.Net.HttpStatusCode.BadRequest);
            PrintRequestAndResponse(input,
                JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync()));
        }
        #endregion

        #region MultipleExpenses
        [Fact]
        public async Task ReturnAllExpenses()
        {
            var response = await _fixture.Client.GetAsync(path + "/" + "?dateoptions=1");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<ExpenseModels.Response>(await response.Content.ReadAsStringAsync());
            _output.WriteLine(responseObject.ToString());
            responseObject.TotalPages.ShouldBeGreaterThan(287);
        }
        [Theory]
        [InlineData("2016-02-14")]
        [InlineData("2020-01-01")]
        [InlineData("2012-01-01")]
        public async Task ReturnsExpensesFromAGivenDate(string date)
        {
            var response = await _fixture.Client.GetAsync(path + "/" + $"?dateoptions=2&beginDate={date}");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<ExpenseModels.Response>(await response.Content.ReadAsStringAsync());
            _output.WriteLine(responseObject.ToString());
            responseObject.PageNumber.ShouldBeGreaterThan(0);
            responseObject.Expenses.Count().ShouldBeGreaterThan(0);
        }
        [Theory]
        [InlineData("2016-02-16")]
        [InlineData("2021-01-01")]
        [InlineData("2012-04-24")]
        public async Task ReturnsExpensesForAGivenMonth(string date)
        {
            var response = await _fixture.Client.GetAsync(path + "/" + $"?dateoptions=3&beginDate={date}");
            response.EnsureSuccessStatusCode();
            var responseObject = JsonConvert.DeserializeObject<ExpenseModels.Response>(await response.Content.ReadAsStringAsync());
            responseObject.PageNumber.ShouldBeGreaterThan(0);
            responseObject.Expenses.Count().ShouldBeGreaterThan(0);

            var expenseList = responseObject.Expenses.ToList();
            var testMonth = DateTime.Parse(date).Month;
            foreach (var exp in expenseList)
            { 
                exp.PurchaseDate.Month.ShouldBe(testMonth);
            }
            _output.WriteLine(responseObject.ToString());
        }
        [Theory]
        [InlineData("2018-02-16")]
        [InlineData("2014-01-01")]
        [InlineData("2013-04-24")]
        public async Task ReturnsExpensesForAGivenQuarter(string date)
        {
            var response = await _fixture.Client.GetAsync(path + "/" + $"?dateoptions=4&beginDate={date}");
            response.EnsureSuccessStatusCode();
            var responseObject = JsonConvert.DeserializeObject<ExpenseModels.Response>(await response.Content.ReadAsStringAsync());
            responseObject.PageNumber.ShouldBeGreaterThan(0);
            responseObject.Expenses.Count().ShouldBeGreaterThan(0);

            var expenseList = responseObject.Expenses.ToList();
            var testYear = DateTime.Parse(date).Year;
            foreach (var exp in expenseList)
            {
                exp.PurchaseDate.Year.ShouldBe(testYear);
            }
            _output.WriteLine(responseObject.ToString());
        }
        [Theory]
        [InlineData("2019-02-16")]
        [InlineData("2017-01-01")]
        [InlineData("2015-04-24")]
        public async Task ReturnsExpensesForAGivenYear(string date)
        {
            var response = await _fixture.Client.GetAsync(path + "/" + $"?dateoptions=5&beginDate={date}");
            response.EnsureSuccessStatusCode();
            var responseObject = JsonConvert.DeserializeObject<ExpenseModels.Response>(await response.Content.ReadAsStringAsync());
            responseObject.PageNumber.ShouldBeGreaterThan(0);
            responseObject.Expenses.Count().ShouldBeGreaterThan(0);

            var expenseList = responseObject.Expenses.ToList();
            var testYear = DateTime.Parse(date).Year;
            foreach (var exp in expenseList)
            {
                exp.PurchaseDate.Year.ShouldBe(testYear);
            }
            _output.WriteLine(responseObject.ToString());
        }
        [Theory]
        [InlineData("2012-03-01", "2012-03-14")]
        [InlineData("2016-11-03", "2021-01-06")]
        [InlineData("2015-04-24", "2016-04-24")]
        public async Task ReturnsExpensesForAGivenDateRange(string beginDate, string endDate)
        {
            var response = await _fixture.Client.GetAsync(path + "/" + $"?dateoptions=6&beginDate={beginDate}&endDate={endDate}");
            response.EnsureSuccessStatusCode();
            var responseObject = JsonConvert.DeserializeObject<ExpenseModels.Response>(await response.Content.ReadAsStringAsync());
            responseObject.PageNumber.ShouldBeGreaterThan(0);
            responseObject.Expenses.Count().ShouldBeGreaterThan(0);

            var expenseList = responseObject.Expenses.ToList();
            foreach (var exp in expenseList)
            {
                exp.PurchaseDate.ShouldBeInRange(DateTimeOffset.Parse(beginDate), DateTimeOffset.Parse(endDate));
            }
            _output.WriteLine(responseObject.ToString());
        }
        [Fact]
        public async Task ReturnsExpensesFromLast30Days()
        {
            var response = await _fixture.Client.GetAsync(path + "/" + $"?dateoptions=7");
            response.EnsureSuccessStatusCode();
            var responseObject = JsonConvert.DeserializeObject<ExpenseModels.Response>(await response.Content.ReadAsStringAsync());
            responseObject.PageNumber.ShouldBeGreaterThan(0);
            responseObject.Expenses.Count().ShouldBeGreaterThan(0);

            var expenseList = responseObject.Expenses.ToList();
            foreach (var exp in expenseList)
            {
                exp.PurchaseDate.ShouldBeGreaterThan(DateTimeOffset.Now.AddDays(-31));
            }
            _output.WriteLine(responseObject.ToString());
        }
        [Fact(Skip ="No expenses entered this month")]
        public async Task ReturnsExpensesForThisMonth()
        {
            var response = await _fixture.Client.GetAsync(path + "/" + $"?dateoptions=8");
            response.EnsureSuccessStatusCode();
            var responseObject = JsonConvert.DeserializeObject<ExpenseModels.Response>(await response.Content.ReadAsStringAsync());
            responseObject.PageNumber.ShouldBeGreaterThan(0);
            responseObject.Expenses.Count().ShouldBeGreaterThan(0);

            var expenseList = responseObject.Expenses.ToList();
            var thisMonth = DateTimeOffset.Now.Month;
            foreach (var exp in expenseList)
            {
                exp.PurchaseDate.Month.ShouldBeEquivalentTo(thisMonth);
            }
            _output.WriteLine(responseObject.ToString());
        }
        [Fact(Skip ="No expenses this quarter")]
        public async Task ReturnsExpensesForThisQuarter()
        {
            var response = await _fixture.Client.GetAsync(path + "/" + $"?dateoptions=9");
            response.EnsureSuccessStatusCode();
            var responseObject = JsonConvert.DeserializeObject<ExpenseModels.Response>(await response.Content.ReadAsStringAsync());
            responseObject.PageNumber.ShouldBeGreaterThan(0);
            responseObject.Expenses.Count().ShouldBeGreaterThan(0);

            var expenseList = responseObject.Expenses.ToList();
            var thisYear = DateTimeOffset.Now.Year;
            foreach (var exp in expenseList)
            {
                exp.PurchaseDate.Year.ShouldBeEquivalentTo(thisYear);
            }
            _output.WriteLine(responseObject.ToString());
        }
        [Fact(Skip = "No expenses this Year")]
        public async Task ReturnsExpensesForThisYear()
        {
            var response = await _fixture.Client.GetAsync(path + "/" + $"?dateoptions=10");
            response.EnsureSuccessStatusCode();
            var responseObject = JsonConvert.DeserializeObject<ExpenseModels.Response>(await response.Content.ReadAsStringAsync());
            responseObject.PageNumber.ShouldBeGreaterThan(0);
            responseObject.Expenses.Count().ShouldBeGreaterThan(0);

            var expenseList = responseObject.Expenses.ToList();
            var thisYear = DateTimeOffset.Now.Year;
            foreach (var exp in expenseList)
            {
                exp.PurchaseDate.Year.ShouldBeEquivalentTo(thisYear);
            }
            _output.WriteLine(responseObject.ToString());
        }
        [Theory]
        [InlineData("2015-04-24", "2016-04-245")]
        [InlineData("2016-04-24", "mitchell")]
        [InlineData("2017-04-24", "2185138642")]

        public async Task ErrorWhenDateRangeSearchDoesntHaveEndDate(string beginDate, string endDate)
        {
            var response = await _fixture.Client.GetAsync(path + "/" + $"?dateoptions=6&beginDate={beginDate}&endDate={endDate}");
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.ShouldContain("EndDate");
            _output.WriteLine(responseString);
        }

        [Theory]
        [InlineData("2016-02-142")]
        [InlineData("Mitchell")]
        [InlineData("2185138642")]
        [EmptyData]
        public async Task ErrorWhenDateSearchDoesntHaveADate(string date)
        {
            var response = await _fixture.Client.GetAsync(path + "/" + $"?dateoptions=2&beginDate={date}");
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.ShouldContain("BeginDate");
            _output.WriteLine(responseString);
        }
        [Theory]
        [InlineData("2016-02-142")]
        [InlineData("Mitchell")]
        [InlineData("2185138642")]
        [EmptyData]
        public async Task ErrorWhenMonthSearchDoesntHaveADate(string date)
        {
            var response = await _fixture.Client.GetAsync(path + "/" + $"?dateoptions=3&beginDate={date}");
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            var responseString = await response.Content.ReadAsStringAsync();
            responseString.ShouldContain("BeginDate");
            _output.WriteLine(responseString);
        }



        [Theory]
        [InlineData("-1")]
        [InlineData("13")]
        [InlineData("#")]
        [EmptyData]
        public async Task ErrorWhenDateOptionsIsntValid(string invalidOptions)
        {
            var response = await _fixture.Client.GetAsync(path + "/" + $"?dateoptions={invalidOptions}");
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var responseString = await response.Content.ReadAsStringAsync();
            responseString.ShouldContain("DateOptions");
            _output.WriteLine(responseString);
        }
        [Theory]
        [InlineData("-100")]
        [InlineData("-4")]
        [InlineData("101")]
        [InlineData("Q")]
        [InlineData("$")]
        [EmptyData]
        public async Task ErrorWhenPageSizeIsntValid(string invalidPageSize)
        {
            var response = await _fixture.Client.GetAsync(path + "/" + $"?dateoptions=1&pageSize={invalidPageSize}");
            var responseString = await response.Content.ReadAsStringAsync();

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            responseString.ShouldContain("PageSize");
            _output.WriteLine(responseString);
        }
        [Theory]
        [InlineData("-100")]
        [InlineData("0")]
        [InlineData("Q")]
        [InlineData("$")]
        [EmptyData]
        public async Task ErrorWhenPageNumberIsntValid(string invalidPageNumber)
        {
            var response = await _fixture.Client.GetAsync(path + "/" + $"?dateoptions=1&pageNumber={invalidPageNumber}");
            var responseString = await response.Content.ReadAsStringAsync();

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            responseString.ShouldContain("PageNumber");
            _output.WriteLine(responseString);
        }
        [Theory]
        [InlineData("1984-04-24")]
        [InlineData("2984-04-24")]
        [InlineData("Mitchell")]
        [InlineData("218-513-8642")]
        [EmptyData]
        public async Task ErrorWhenBeginDateIsntValid(string invalidDate)
        {
            var response = await _fixture.Client.GetAsync(path + "/" + $"?dateoptions=1&beginDate={invalidDate}");
            var responseString = await response.Content.ReadAsStringAsync();

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            responseString.ShouldContain("BeginDate");
            _output.WriteLine(responseString);
        }
        [Theory]
        [InlineData("1984-04-24")]
        [InlineData("2984-04-24")]
        [InlineData("Mitchell")]
        [InlineData("218-513-8642")]
        [EmptyData]
        public async Task ErrorWhenEndDateIsntValid(string invalidDate)
        {
            var response = await _fixture.Client.GetAsync(path + "/" + $"?dateoptions=1&endDate={invalidDate}");
            var responseString = await response.Content.ReadAsStringAsync();

            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
            responseString.ShouldContain("EndDate");
            _output.WriteLine(responseString);
        }
        #endregion

        private void PrintRequestAndResponse(object request, object response)
        {
            _output.WriteLine(request.ToString());
            _output.WriteLine(response.ToString());
        }
        private ExpenseModels.Request GetExpenseRequest()
        {
            return new ExpenseModels.Request();
        }
    }
}
