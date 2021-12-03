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
        #region SingleId
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
        [Fact]
        public async Task ReturnAllExpenses()
        {
            var response = await _fixture.Client.GetAsync(path + "/" + "?dateoptions=1");
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<Expense.Response>(await response.Content.ReadAsStringAsync());
            _output.WriteLine(responseObject.ToString());
            responseObject.TotalPages.ShouldBeGreaterThan(287);
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


        private void PrintRequestAndResponse(object request, object response)
        {
            _output.WriteLine(request.ToString());
            _output.WriteLine(response.ToString());
        }
        private Expense.Request GetExpenseRequest()
        {
            return new Expense.Request();
        }
    }
}
