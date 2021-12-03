﻿using CashTrack.IntegrationTests.Common;
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
        public async void ReturnAnErrorWithInvalidId(int id)
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
        public async void ReturnAnErrorWithInvalidInput(object input)
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

            _output.WriteLine(responseString);

            Assert.Contains($"\"id\"", responseString);
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