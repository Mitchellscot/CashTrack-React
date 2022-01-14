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
    public class MerchantControllerShould : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;
        private ITestOutputHelper _output;
        const string path = "api/merchant";

        public MerchantControllerShould(TestServerFixture fixture, ITestOutputHelper output)
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

        //randomize these numbers
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public async Task ReturnMerchantWithMatchingId(int id)
        {
            var response = await _fixture.Client.GetAsync(path + $"/{id}");
            response.EnsureSuccessStatusCode();
            var responseObject = JsonConvert.DeserializeObject<MerchantModels.Merchant>(await response.Content.ReadAsStringAsync());
            responseObject.ShouldNotBeNull();
            PrintRequestAndResponse(path + $"/{id}", await response.Content.ReadAsStringAsync());
        }
        [Theory]
        [InlineData(0)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public async Task ThrowExceptionWithInvalidId(int id)
        {
            var response = await _fixture.Client.GetAsync(path + "/" + id);
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
