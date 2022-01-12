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
