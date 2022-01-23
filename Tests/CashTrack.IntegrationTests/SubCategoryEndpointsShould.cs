using CashTrack.Models.SubCategoryModels;
using Newtonsoft.Json;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace CashTrack.IntegrationTests
{
    public class SubCategoryEndpointsShould : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;
        private readonly ITestOutputHelper _output;
        const string path = "api/subcategory";

        public SubCategoryEndpointsShould(TestServerFixture fixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            _output = output;
        }
        [Fact]
        public async Task ReturnAllCategories()
        {
            var response = await _fixture.Client.GetAsync(path);
            response.EnsureSuccessStatusCode();
            var responseObject = JsonConvert.DeserializeObject<SubCategoryModels.Response>(await response.Content.ReadAsStringAsync());
            _output.WriteLine(await response.Content.ReadAsStringAsync());
            responseObject.TotalPages.ShouldBeGreaterThan(1);
            responseObject.TotalSubCategories.ShouldBe(73);
            responseObject.PageNumber.ShouldBe(1);
            responseObject.SubCategories.ShouldNotBeEmpty<SubCategoryListItem>();
        }
        [Fact]
        public async Task ReturnAllCategoriesWithPagination()
        {
            var response = await _fixture.Client.GetAsync(path + "?pageNumber=2&pageSize=50");
            response.EnsureSuccessStatusCode();
            var responseObject = JsonConvert.DeserializeObject<SubCategoryModels.Response>(await response.Content.ReadAsStringAsync());
            _output.WriteLine(await response.Content.ReadAsStringAsync());
            responseObject.TotalPages.ShouldBeGreaterThan(1);
            responseObject.TotalSubCategories.ShouldBe(73);
            responseObject.PageNumber.ShouldBe(2);
            responseObject.SubCategories.ShouldNotBeEmpty<SubCategoryListItem>();
        }
        [Theory]
        [InlineData("doc")]
        [InlineData("soft")]
        [InlineData("car")]
        public async Task ReturnSubCategoriesWithMatchingSearchTerm(string searchTerm)
        {
            var response = await _fixture.Client.GetAsync(path + $"?searchterm={searchTerm}");
            response.EnsureSuccessStatusCode();
            var responseObject = JsonConvert.DeserializeObject<SubCategoryModels.Response>(await response.Content.ReadAsStringAsync());
            _output.WriteLine(await response.Content.ReadAsStringAsync());
            responseObject.TotalPages.ShouldBeGreaterThanOrEqualTo(1);
            responseObject.TotalSubCategories.ShouldBeGreaterThan(1);
            responseObject.SubCategories.ShouldNotBeEmpty<SubCategoryListItem>();
        }
    }
}
