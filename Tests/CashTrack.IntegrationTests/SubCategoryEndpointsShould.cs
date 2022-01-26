﻿using CashTrack.Models.SubCategoryModels;
using Newtonsoft.Json;
using Shouldly;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace CashTrack.IntegrationTests
{
    public class SubCategoryEndpointsShould : IClassFixture<TestServerFixture>
    {
        private readonly TestServerFixture _fixture;
        private readonly ITestOutputHelper _output;
        const string ENDPOINT = "api/subcategory";

        public SubCategoryEndpointsShould(TestServerFixture fixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            _output = output;
        }
        [Fact]
        public async Task ReturnAllCategories()
        {
            var response = await _fixture.Client.GetAsync(ENDPOINT);
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
            var response = await _fixture.Client.GetAsync(ENDPOINT + "?pageNumber=2&pageSize=50");
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
            var response = await _fixture.Client.GetAsync(ENDPOINT + $"?searchterm={searchTerm}");
            response.EnsureSuccessStatusCode();
            var responseObject = JsonConvert.DeserializeObject<SubCategoryModels.Response>(await response.Content.ReadAsStringAsync());
            _output.WriteLine(await response.Content.ReadAsStringAsync());
            responseObject.TotalPages.ShouldBeGreaterThanOrEqualTo(1);
            responseObject.TotalSubCategories.ShouldBeGreaterThan(1);
            responseObject.SubCategories.ShouldNotBeEmpty<SubCategoryListItem>();
        }
        [Fact]
        public async Task CreateUpdateDeleteSubCategories()
        {
            var testId = 0;
            try
            {
                //create
                var uniqueName = Guid.NewGuid().ToString();
                var request = new AddEditSubCategory() with { Name = uniqueName, InUse = true, MainCategoryId = 12 };
                var response = await _fixture.SendPostRequestAsync(ENDPOINT, request);
                response.StatusCode.ShouldBe(HttpStatusCode.Created);
                var responseObject = JsonConvert.DeserializeObject<AddEditSubCategory>(await response.Content.ReadAsStringAsync());
                testId = responseObject.Id!.Value;
                response.Headers.Location!.ToString().ShouldContain(responseObject.Id.ToString()!);
                responseObject.Name.ShouldBe(uniqueName);
                //update
                var updatedObject = new AddEditSubCategory() with { Id = testId, Name = Guid.NewGuid().ToString(), MainCategoryId = 12, InUse = false };
                var updatedResponse = await _fixture.SendPutRequestAsync(ENDPOINT, updatedObject);
                updatedResponse.EnsureSuccessStatusCode();
            }
            finally
            {
                //delete
                var deleteResponse = await _fixture.Client.DeleteAsync(ENDPOINT + $"/{testId}");
                deleteResponse.EnsureSuccessStatusCode();
            }
        }
    }
}
