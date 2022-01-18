using CashTrack.Models.MerchantModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Shouldly;
using Newtonsoft.Json;
using System.Net;

namespace CashTrack.IntegrationTests
{
    public class MerchantsControllerShould : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;
        private ITestOutputHelper _output;
        const string path = "api/merchants";

        public MerchantsControllerShould(TestServerFixture fixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            _output = output;
        }
        [Fact]
        public async Task ReturnAllMerchants()
        { 
            var response = await _fixture.Client.GetAsync(path);
            response.EnsureSuccessStatusCode();
            var responseObject = JsonConvert.DeserializeObject<MerchantModels.Response>(await response.Content.ReadAsStringAsync());
            _output.WriteLine(await response.Content.ReadAsStringAsync());
            responseObject.TotalPages.ShouldBeGreaterThan(19);
            responseObject.Merchants.Count().ShouldBe(25);
            responseObject.PageNumber.ShouldBe(1);
        }
        [Fact]
        public async Task ReturnAllMerchantsWithPagination()
        {
            var response = await _fixture.Client.GetAsync(path + "?pageNumber=2&pageSize=50");
            response.EnsureSuccessStatusCode();
            var responseObject = JsonConvert.DeserializeObject<MerchantModels.Response>(await response.Content.ReadAsStringAsync());
            _output.WriteLine(await response.Content.ReadAsStringAsync());
            responseObject.TotalPages.ShouldBeGreaterThan(9);
            responseObject.Merchants.Count().ShouldBe(50);
            responseObject.PageNumber.ShouldBe(2);
        }
        [Theory]
        [InlineData("Costco")]
        [InlineData("John")]
        [InlineData("Home")]
        public async Task ReturnMerchantsWithMatchingSearchTerm(string searchTerm)
        {
            var response = await _fixture.Client.GetAsync(path + $"?searchterm={searchTerm}");
            response.EnsureSuccessStatusCode();
            var responseObject = JsonConvert.DeserializeObject<MerchantModels.Response>(await response.Content.ReadAsStringAsync());
            responseObject.Merchants.Count().ShouldBeGreaterThan(1);
            responseObject.Merchants.First().Name.ShouldContain(searchTerm);
            PrintRequestAndResponse(path + $"?searchterm={searchTerm}", await response.Content.ReadAsStringAsync());
        }

        [Theory]
        [InlineData(17)]
        [InlineData(85)]
        [InlineData(350)]
        public async Task ReturnMerchantDetail(int id)
        {
            var expectedMerchantNames = new List<string>() { "Amazon", "Costco", "Walmart" };
            var response = await _fixture.Client.GetAsync(path + $"/detail/{id}");
            response.EnsureSuccessStatusCode();
            var responseObject = JsonConvert.DeserializeObject<MerchantDetail>(await response.Content.ReadAsStringAsync());
            responseObject.ShouldNotBeNull();
            expectedMerchantNames.ShouldContain(responseObject.Name);
            responseObject.AnnualExpenseStatistics.ForEach(x => x.Average.ShouldBe(Math.Round(x.Total / x.Count, 2)));
            responseObject.AnnualExpenseStatistics.ForEach(x => x.Year.ShouldBeGreaterThan(2011));
            responseObject.AnnualExpenseStatistics.ForEach(x => x.Max.ShouldBeGreaterThan(0));
            responseObject.AnnualExpenseStatistics.ForEach(x => x.Min.ShouldBeGreaterThan(0));
            responseObject.ExpenseTotals.TotalSpentAllTime.ShouldBeGreaterThan(1);
            responseObject.MostUsedCategory.ShouldNotBeEmpty();
            responseObject.PurchaseCategoryOccurances.ShouldNotBeEmpty();
            responseObject.PurchaseCategoryTotals.ShouldNotBeEmpty();
            responseObject.RecentExpenses.Count.ShouldBeGreaterThan(0);
            PrintRequestAndResponse(path + $"/{id}", await response.Content.ReadAsStringAsync());
        }
        [Theory]
        [InlineData(0)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public async Task ThrowExceptionWithInvalidId(int id)
        {
            var response = await _fixture.Client.GetAsync(path + $"/detail/{id}");
            response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains($"No merchant found with an id of {id}", responseString);
            PrintRequestAndResponse(path + $"/99999999", await response.Content.ReadAsStringAsync());
        }
        private void PrintRequestAndResponse(object request, object response)
        {
            _output.WriteLine(request.ToString());
            _output.WriteLine(response.ToString());
        }
        private MerchantModels.Request GetMerchantsRequest()
        {
            return new MerchantModels.Request();
        }
    }
}
